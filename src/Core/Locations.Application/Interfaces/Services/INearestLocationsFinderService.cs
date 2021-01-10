
using System.Collections.Generic;
using System.Threading.Tasks;

using Locations.Core.Application.Features.Locations.Queries.GetNearestLocations;
using Locations.Core.Application.Models;

namespace Locations.Core.Application.Interfaces.Services
{
    public interface INearestLocationsFinderService
    {
        Task<IEnumerable<LocationWithDistanceFromStartingPoint>> GetNearestLocations(StartingLocation fromLocation, int maxDistance, int maxResults);
    }
}
