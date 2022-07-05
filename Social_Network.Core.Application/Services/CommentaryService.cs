using AutoMapper;
using Microsoft.AspNetCore.Http;
using Social_Network.Core.Application.Helpers;
using Social_Network.Core.Application.Interfaces.Repositories;
using Social_Network.Core.Application.Interfaces.Services;
using Social_Network.Core.Application.ViewModels.Commentary;
using Social_Network.Core.Application.ViewModels.User;
using Social_Network.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.Services
{
    public class CommentaryService : GenericService<CommentaryViewModel, CommentaryViewModel, Commentary>, ICommentaryService
    {
        private readonly ICommentaryRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommentaryService(ICommentaryRepository repo, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(repo, mapper)
        {
            _repository = repo;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public override async Task<CommentaryViewModel> Add(CommentaryViewModel postVm)
        {
            postVm.UserId = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user").Id;

            return await base.Add(postVm);
        }
    }
}
