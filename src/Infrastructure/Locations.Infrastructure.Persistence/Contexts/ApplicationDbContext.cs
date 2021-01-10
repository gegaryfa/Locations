using Locations.Core.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Locations.Infrastructure.Persistence.Contexts
{
    public sealed class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<Location> Locations { get; set; }
    }
}
