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
    public class FriendService : GenericService<FriendViewModel, FriendViewModel, Friend>, IFriendService
    {
        private readonly IFriendRepository _repository;
        private readonly IMapper _mapper;

        public FriendService(IFriendRepository repo, IMapper mapper) : base(repo, mapper)
        {
            _repository = repo;
            _mapper = mapper;
        }

        public async Task<List<FriendViewModel>> GetAllViewModelWithInclude()
        {
            var entityList = await _repository.GetAllWithIncludeAsync(new List<string> { "User", "UserFriend" });

            List<FriendViewModel> friendList = entityList.Select(friend => new FriendViewModel
            {
                Id = friend.Id,
                UserId = friend.UserId,
                FriendsWith = friend.FriendsWith,
                User = _mapper.Map<UserViewModel>(friend.User),
                UserFriend = _mapper.Map<UserViewModel>(friend.UserFriend),
                DateCreated = friend.DateCreated
            }).ToList();

            List<FriendViewModel> sortedList = friendList.OrderByDescending(x => x.DateCreated)
                .ToList();

            return sortedList;
        }
    }
}
