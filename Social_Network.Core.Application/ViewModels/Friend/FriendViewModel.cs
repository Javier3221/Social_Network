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
        public int FriendFrom { get; set; }
        public int FriendTo { get; set; }

        public UserViewModel User { get; set; }
        public UserViewModel UserFriend { get; set; }
    }
}
