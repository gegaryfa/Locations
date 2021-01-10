using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Locations.Core.Application.Features.Locations.Queries.GetNearestLocations;
using Locations.Core.Application.Interfaces.Services;
using Locations.Core.Application.Models;
using Locations.Core.Domain.Entities;
using Locations.Core.Domain.Interfaces.Repositories;

namespace Locations.Infrastructure.Shared.Services
{
    public class NearestLocationsFinderService : INearestLocationsFinderService
    {
        private readonly ILocationsRepositoryAsync _locationsRepository;

        public NearestLocationsFinderService(ILocationsRepositoryAsync locationsRepository)
        {
            _locationsRepository = locationsRepository;
        }

        public async Task<IEnumerable<LocationWithDistanceFromStartingPoint>> GetNearestLocations(StartingLocation startingLocation, int maxDistance, int maxResults)
        {
            // In the case of an actual database with lots of records, we can use caching to save time when 
            // fetching al the records.
            var allLocations = await _locationsRepository.GetAllAsync();

            var startLoc = new Location(startingLocation.Latitude, startingLocation.Longitude);

            // Time complexity depends on the size of M. Worst case if M is big then TC=O(M log M), else O(N)
            var res = allLocations
                .Select(l => new LocationWithDistanceFromStartingPoint(l, l.CalculateDistance(startLoc))) // Select: O(N)
                .Where(l => l.DistanceFromStartingPoint <= maxDistance) // Where: O(N)
                .OrderBy(l => l.DistanceFromStartingPoint) //O(M log M)
                .Take(maxResults); // Take: O(M)

            return res;
        }


    }
}
