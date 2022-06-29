using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.ViewModels.Commentary
{
    public class CommentaryViewModel
    {
        public string Description { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
    }
}
