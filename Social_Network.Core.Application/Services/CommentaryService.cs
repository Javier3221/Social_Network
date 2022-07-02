using AutoMapper;
using Social_Network.Core.Application.Interfaces.Repositories;
using Social_Network.Core.Application.Interfaces.Services;
using Social_Network.Core.Application.ViewModels.Commentary;
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

        public CommentaryService(ICommentaryRepository repo, IMapper mapper) : base(repo, mapper)
        {
            _repository = repo;
            _mapper = mapper;
        }
    }
}
