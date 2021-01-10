using Locations.Core.Domain.Entities;
using Locations.Core.Domain.Interfaces.Repositories;
using Locations.Infrastructure.Persistence.Contexts;

using Microsoft.EntityFrameworkCore;

namespace Locations.Infrastructure.Persistence.Repositories
{
    public class LocationsRepositoryAsync : GenericRepositoryAsync<Location>, ILocationsRepositoryAsync
    {
        private readonly DbSet<Location> _locations;

        public LocationsRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _locations = dbContext.Set<Location>();
        }
    }
}
