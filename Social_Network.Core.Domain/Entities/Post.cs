using Social_Network.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Domain.Entities
{
    public class Post : AuditableProperties
    {
        public string PostDescription { get; set; }
        public string ImgUrl { get; set; }
        public int UserId { get; set; }

        //Nav properties
        public User User { get; set; }
        public ICollection<Commentary> Commentaries { get; set; }
    }
}
