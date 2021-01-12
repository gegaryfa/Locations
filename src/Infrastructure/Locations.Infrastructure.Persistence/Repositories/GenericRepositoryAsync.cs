using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using EnsureThat;

using Locations.Core.Domain.Interfaces.Repositories;
using Locations.Infrastructure.Persistence.Contexts;

using Microsoft.EntityFrameworkCore;

namespace Locations.Infrastructure.Persistence.Repositories
{
    public class GenericRepositoryAsync<T> : IGenericRepositoryAsync<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;

        public GenericRepositoryAsync(ApplicationDbContext dbContext)
        {
            EnsureArg.IsNotNull(dbContext, nameof(dbContext));

            _dbContext = dbContext;
        }

        public async Task<T> AddAsync(T entity)
        {
            EnsureArg.IsNotNull(entity, nameof(entity));

            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext
                 .Set<T>()
                 .ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetPagedResponseAsync(int pageNumber, int pageSize)
        {
            EnsureArg.IsGte(pageNumber, 1, nameof(pageNumber));
            EnsureArg.IsGte(pageSize, 1, nameof(pageSize));

            return await _dbContext
                .Set<T>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public IReadOnlyList<T> GetAll()
        {
            return _dbContext
                .Set<T>()
                .ToList();
        }
    }
}
