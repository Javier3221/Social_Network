using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string ProfileImgUrl { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool ActivatedAccount { get; set; }

        //Nav properties
        public ICollection<Friend> Friends { get; set; }
        public ICollection<Friend> FriendWith { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}
