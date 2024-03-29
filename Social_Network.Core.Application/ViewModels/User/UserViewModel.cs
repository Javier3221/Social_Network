﻿using Social_Network.Core.Application.ViewModels.Friend;
using Social_Network.Core.Application.ViewModels.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.ViewModels.User
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string ProfileImgUrl { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<string> FriendUserNames { get; set; }
        public bool ActivatedAccount { get; set; }

        public ICollection<FriendViewModel> Friends { get; set; }
        public ICollection<FriendViewModel> FriendWith { get; set; }
        public ICollection<PostViewModel> Posts { get; set; }
    }
}
