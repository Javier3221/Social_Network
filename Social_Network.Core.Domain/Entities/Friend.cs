using Social_Network.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Domain.Entities
{
    public class Friend : AuditableProperties
    {
        public int UserId { get; set; }
        public int FriendsWith { get; set; }

        //Nav properties
        public User User { get; set; }
        public User UserFriend { get; set; }
    }
}
