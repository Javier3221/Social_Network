using AutoMapper;
using Social_Network.Core.Application.Interfaces.Repositories;
using Social_Network.Core.Application.Interfaces.Services;
using Social_Network.Core.Application.ViewModels.Post;
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

        public PostService(IPostRepository postRepository, IMapper mapper) : base(postRepository, mapper)
        {
            _repository = postRepository;
            _mapper = mapper;
        }

        public async Task Update(SavePostViewModel vm, int id)
        {
            Post post = await _repository.GetByIdAsync(vm.Id);
            post.Id = vm.Id;
            post.PostDescription = vm.PostDescription;

            await _repository.UpdateAsync(post, id);
        }
    }
}
