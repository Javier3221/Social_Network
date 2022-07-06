using AutoMapper;
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

        public UserService(IUserRepository repo, IMapper mapper) : base(repo, mapper)
        {
            _repository = repo;
            _mapper = mapper;
        }

        public override async Task<SaveUserViewModel> Add(SaveUserViewModel vm)
        {
            vm.ActivatedAccount = false;

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

        public async Task<List<UserViewModel>> GetAllViewModelWithInclude()
        {
            var userList = await _repository.GetAllWithIncludeAsync(new List<string> { "Friends" });

            return userList.Select(user => new UserViewModel 
            {
                Name = user.Name,
                LastName = user.LastName,
                Phone = user.Phone,
                Password = user.Password,
                ProfileImgUrl = user.ProfileImgUrl,
                Email = user.Email,
                ActivatedAccount = user.ActivatedAccount,
                UserName = user.UserName
            }).ToList();
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
