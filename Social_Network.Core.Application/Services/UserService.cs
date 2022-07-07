using AutoMapper;
using Social_Network.Core.Application.DTOs.Email;
using Social_Network.Core.Application.Interfaces.Repositories;
using Social_Network.Core.Application.Interfaces.Services;
using Social_Network.Core.Application.ViewModels.Friend;
using Social_Network.Core.Application.ViewModels.User;
using Social_Network.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.Services
{
    public class UserService : GenericService<SaveUserViewModel, UserViewModel, User>, IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public UserService(IUserRepository repo, IMapper mapper, IEmailService emailService) : base(repo, mapper)
        {
            _repository = repo;
            _mapper = mapper;
            _emailService = emailService;
        }

        public override async Task<SaveUserViewModel> Add(SaveUserViewModel vm)
        {
            vm.ActivatedAccount = false;
            string localUrl = "https://localhost:44391/User/ActivateAccount/";
            await _emailService.SendAsync(new EmailRequest
            {
                To = vm.Email,
                Subject = "Welcome to PoroNet",
                Body = $"<h1>Welcome to PoroNet</h1> <p>We're really glad you chose us \"{vm.UserName}\"</p>" +
                $"<br/><a href='{localUrl}'>Click this link to activate your account</a>"
            });

            return await base.Add(vm);
        }

        public async Task<UserViewModel> Login(LoginViewModel loginVm)
        {
            User user = await _repository.LoginAsync(loginVm);

            if (user == null)
            {
                return null;
            }

            UserViewModel vm = _mapper.Map<UserViewModel>(user);

            return vm;
        }

        public async Task<bool> FindUserNameAvailabilty(string userName)
        {
            return await _repository.FindUserNameAvailabilty(userName);
        }

        public async Task<UserViewModel> FindUserByUserName(string userName)
        {
            UserViewModel userVm = _mapper.Map<UserViewModel>(await _repository.FindUserByUserName(userName));
            UserViewModel withIncludes = await GetByIdWithInclude(userVm.Id);
            userVm.FriendUserNames = withIncludes.FriendUserNames;
            return userVm;
        }

        public async Task<SaveUserViewModel> FindUserByEmail(string email)
        {
            SaveUserViewModel user = _mapper.Map<SaveUserViewModel>(await _repository.FindUserByEmail(email));

            return user;
        }

        public async Task<List<UserViewModel>> GetAllViewModelWithInclude()
        {
            List<UserViewModel> userList = _mapper.Map<List<UserViewModel>>(await _repository.GetAllWithIncludeAsync(new List<string> { "Friends" }));

            return userList;
        }

        public async Task<UserViewModel> GetByIdWithInclude(int id)
        {
            var userList = await _repository.GetAllWithIncludeAsync(new List<string> { "Friends", "FriendWith" });

            UserViewModel user = _mapper.Map<UserViewModel>(userList.FirstOrDefault(user => user.Id == id));
            user.FriendUserNames = new();
            List<FriendViewModel> friends = user.Friends as List<FriendViewModel>;
            List<FriendViewModel> friendWith = user.FriendWith as List<FriendViewModel>;

            for (int i = 0; i < friends.Count; i++)
            {
                if (user.Id == friends[i].UserId)
                {
                    user.FriendUserNames.Add(friends[i].UserFriend.UserName);
                }
                else
                {
                    user.FriendUserNames.Add(friends[i].User.UserName);
                }
            }

            for (int i = 0; i < friendWith.Count; i++)
            {
                if (user.Id == friendWith[i].UserId)
                {
                    user.FriendUserNames.Add(friendWith[i].UserFriend.UserName);
                }
                else
                {
                    user.FriendUserNames.Add(friendWith[i].User.UserName);
                }
            }

            return user;
        }
    }
}
