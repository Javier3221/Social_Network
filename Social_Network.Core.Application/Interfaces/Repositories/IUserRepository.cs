using Social_Network.Core.Application.ViewModels.User;
using Social_Network.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.Interfaces.Repositories
{
    public interface IUserRepository : IGenericRepositoryAsync<User>
    {
        Task<bool> FindUserNameAvailabilty(string userName);
        Task<User> LoginAsync(LoginViewModel entity);
    }
}
