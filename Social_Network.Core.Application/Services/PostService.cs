using AutoMapper;
using Microsoft.AspNetCore.Http;
using Social_Network.Core.Application.Helpers;
using Social_Network.Core.Application.Interfaces.Repositories;
using Social_Network.Core.Application.Interfaces.Services;
using Social_Network.Core.Application.ViewModels.Commentary;
using Social_Network.Core.Application.ViewModels.Friend;
using Social_Network.Core.Application.ViewModels.Post;
using Social_Network.Core.Application.ViewModels.User;
using Social_Network.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.Services
{
    public class PostService : GenericService<SavePostViewModel, PostViewModel, Post> ,IPostService
    {
        private readonly IPostRepository _repository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserViewModel currentUser;

        public PostService(IPostRepository postRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IUserService userService) : base(postRepository, mapper)
        {
            _repository = postRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            currentUser = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
        }

        public override async Task<SavePostViewModel> Add(SavePostViewModel saveVm)
        {
            saveVm.UserId = currentUser.Id;

            return await base.Add(saveVm);
        }

        public override async Task Update(SavePostViewModel saveVm, int id)
        {
            saveVm.UserId = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user").Id;

            await base.Update(saveVm, id);
        }

        public async Task<List<PostViewModel>> GetAllViewModelWithInclude()
        {
            var entityList = await _repository.GetAllWithIncludeAsync(new List<string> { "User", "Commentaries" });

            List<PostViewModel> postList = entityList.Select(post => new PostViewModel
            {
                PostDescription = post.PostDescription,
                UserId = post.UserId,
                Id = post.Id,
                ImgUrl = post.ImgUrl,
                User = _mapper.Map<UserViewModel>(post.User),
                Commentaries = _mapper.Map<List<CommentaryViewModel>>(post.Commentaries),
                DateCreated = post.DateCreated
            }).ToList();

            List<PostViewModel> sortedList = postList.OrderByDescending(x => x.DateCreated)
                .ToList();

            return sortedList;
        }

        public async Task<List<PostViewModel>> GetAllUserPosts()
        {
            var postList = await GetAllViewModelWithInclude();

            return postList.Where(post => post.UserId == currentUser.Id).ToList();
        }

        public async Task<List<PostViewModel>> GetAllFriendPosts()
        {
            var postList = await GetAllViewModelWithInclude();

            List<PostViewModel> friendPostList = new();
            UserViewModel user = await _userService.GetByIdWithInclude(currentUser.Id);

            foreach (FriendViewModel friend in user.Friends)
            {
                if (friend.UserId == user.Id)
                {
                    friendPostList.AddRange(
                    postList.Where(elem => elem.UserId == friend.FriendsWith)
                    );
                }
                else
                {
                    friendPostList.AddRange(
                    postList.Where(elem => elem.UserId == friend.UserId)
                    );
                }
            }

            foreach (FriendViewModel friend in user.FriendWith)
            {
                if (friend.UserId == user.Id)
                {
                    friendPostList.AddRange(
                    postList.Where(elem => elem.UserId == friend.FriendsWith)
                    );
                }
                else
                {
                    friendPostList.AddRange(
                    postList.Where(elem => elem.UserId == friend.UserId)
                    );
                }
            }

            List<PostViewModel> sortedList = friendPostList.OrderByDescending(x => x.DateCreated)
                .ToList();

            return sortedList;
        }
    }
}
