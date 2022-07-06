using Social_Network.Core.Application.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.ViewModels.Friend
{
    public class FriendViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FriendsWith { get; set; }
        public DateTime DateCreated { get; set; }

        public UserViewModel User { get; set; }
        public UserViewModel UserFriend { get; set; }
    }
}
