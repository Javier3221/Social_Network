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
    public class FriendRepository : GenericRepository<Friend>, IFriendRepository
    {
        private readonly ApplicationContext _dbContext;
        public FriendRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
