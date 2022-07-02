using Microsoft.EntityFrameworkCore;
using Social_Network.Core.Application.Interfaces.Repositories;
using Social_Network.Core.Domain.Entities;
using Social_Network.Infrastructure.Persistence.Contexts;
using Social_Network.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Infrastructure.Persistence.Repositories
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        private readonly ApplicationContext _dbContext;
        public PostRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task UpdateAsync(Post post, int id)
        {
            Post entry = await _dbContext.Set<Post>().FindAsync(id);
            _dbContext.Entry(entry).CurrentValues.SetValues(post);
            await _dbContext.SaveChangesAsync();
        }
    }
}
