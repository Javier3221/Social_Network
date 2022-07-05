using AutoMapper;
using Microsoft.AspNetCore.Http;
using Social_Network.Core.Application.Helpers;
using Social_Network.Core.Application.Interfaces.Repositories;
using Social_Network.Core.Application.Interfaces.Services;
using Social_Network.Core.Application.ViewModels.Commentary;
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
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly int currentUserId;

        public PostService(IPostRepository postRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(postRepository, mapper)
        {
            _repository = postRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            currentUserId = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user").Id;
        }

        public override async Task<SavePostViewModel> Add(SavePostViewModel saveVm)
        {
            saveVm.UserId = currentUserId;

            return await base.Add(saveVm);
        }

        public override async Task Update(SavePostViewModel saveVm, int id)
        {
            saveVm.UserId = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user").Id;

            await base.Update(saveVm, id);
        }

        public async Task<List<PostViewModel>> GetAllViewModelWithInclude()
        {
            var entityList = await _repository.GetAllWithIncludeAsync(new List<string> { "User", "Commentaries", "Friends" });

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

            List<PostViewModel> sortedList = postList.OrderBy(x => x.DateCreated.Year)
                .ThenBy(x => x.DateCreated.Month)
                .ThenBy(x => x.DateCreated.Day)
                .ToList();

            return sortedList;
        }

        public async Task<List<PostViewModel>> GetAllUserPosts()
        {
            var postList = await GetAllViewModelWithInclude();

            return postList.Where(post => post.UserId == currentUserId).ToList();
        }
    }
}
