using Social_Network.Core.Application.ViewModels.Post;
using Social_Network.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.Interfaces.Services
{
    public interface IPostService : IGenericService<SavePostViewModel, PostViewModel, Post>
    {
        Task<List<PostViewModel>> GetAllViewModelWithInclude();
        Task<List<PostViewModel>> GetAllUserPosts();
        Task<List<PostViewModel>> GetAllFriendPosts();
    }
}
