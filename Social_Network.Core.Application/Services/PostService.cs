using AutoMapper;
using Microsoft.AspNetCore.Http;
using Social_Network.Core.Application.Helpers;
using Social_Network.Core.Application.Interfaces.Repositories;
using Social_Network.Core.Application.Interfaces.Services;
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

        public PostService(IPostRepository postRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(postRepository, mapper)
        {
            _repository = postRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public override async Task<SavePostViewModel> Add(SavePostViewModel saveVm)
        {
            saveVm.UserId = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user").Id;

            return await base.Add(saveVm);
        }
    }
}
