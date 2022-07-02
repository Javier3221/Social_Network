﻿using Social_Network.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.Interfaces.Repositories
{
    public interface IPostRepository : IGenericRepositoryAsync<Post>
    {
        Task UpdateAsync(Post post, int id);
    }
}
