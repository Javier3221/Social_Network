﻿using Social_Network.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Domain.Entities
{
    public class Commentary : AuditableProperties
    {
        public string Description { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }

        //nav properties
        public Post Post { get; set; }
        public User User { get; set; }
    }
}