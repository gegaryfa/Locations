using System.Collections.Generic;
using System.Threading.Tasks;

namespace Locations.Core.Domain.Interfaces.Repositories
{
    public interface IGenericRepositoryAsync<T> where T : class
    {

        Task<IReadOnlyList<T>> GetAllAsync();

        Task<IReadOnlyList<T>> GetPagedResponseAsync(int pageNumber, int pageSize);

        IReadOnlyList<T> GetAll();

        Task<T> AddAsync(T entity);
    }
}
