using Locations.Core.Application.Interfaces.Services;
using Locations.Infrastructure.Shared.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Locations.Infrastructure.Shared
{
    public static class ServiceRegistration
    {
        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration _config)
        {
            services.AddTransient<INearestLocationsFinderService, NearestLocationsFinderServiceV2>();
        }
    }
}
