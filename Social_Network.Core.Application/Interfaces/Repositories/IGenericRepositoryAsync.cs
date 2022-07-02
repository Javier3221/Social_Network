using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.Interfaces.Repositories
{
    public interface IGenericRepositoryAsync<Entity> where Entity : class
    {
        Task<Entity> AddAsync(Entity entity);
        Task DeleteAsync(Entity entity);
        Task<List<Entity>> GetAllAsync();
        Task<List<Entity>> GetAllWithIncludeAsync(List<string> properties);
        Task<Entity> GetByIdAsync(int id);
    }
}
