using Microsoft.EntityFrameworkCore;
using Social_Network.Core.Application.Helpers;
using Social_Network.Core.Application.Interfaces.Repositories;
using Social_Network.Core.Application.ViewModels.User;
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
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly ApplicationContext _dbContext;
        public UserRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<User> AddAsync(User entity)
        {
            entity.Password = PasswordEncryption.ComputeSha256Hash(entity.Password);
            return await base.AddAsync(entity);
        }

        public async Task<bool> FindUserNameAvailabilty(string userName)
        {
            bool IsAvailable = await _dbContext.Set<User>().FirstOrDefaultAsync(user => user.UserName == userName) == null;
            return IsAvailable;
        }
        
        public async Task<User> FindUserByUserName(string userName)
        {
            User user = await _dbContext.Set<User>()
                .FirstOrDefaultAsync(user => user.UserName == userName);
            return user;
        }

        public async Task<User> LoginAsync(LoginViewModel entity)
        {
            string passwordEncrypted = PasswordEncryption.ComputeSha256Hash(entity.Password);
            User user = await _dbContext.Set<User>()
                .FirstOrDefaultAsync(user => user.UserName == entity.UserName && user.Password == passwordEncrypted);
            return user;
        }
    }
}
