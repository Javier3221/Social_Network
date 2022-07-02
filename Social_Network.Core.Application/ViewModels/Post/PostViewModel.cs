using Social_Network.Core.Application.ViewModels.Commentary;
using Social_Network.Core.Application.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.ViewModels.Post
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string PostDescription { get; set; }
        public string ImgUrl { get; set; }
        public int UserId { get; set; }

        public UserViewModel User { get; set; }
        public ICollection<CommentaryViewModel> Commentaries { get; set; }
    }
}
