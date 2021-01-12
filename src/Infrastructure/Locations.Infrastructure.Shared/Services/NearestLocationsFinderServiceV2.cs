using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using EnsureThat;

using Locations.Core.Application.DTOs;
using Locations.Core.Application.Features.Locations.Queries.GetNearestLocations;
using Locations.Core.Application.Interfaces.Services;
using Locations.Core.Domain.Entities;
using Locations.Infrastructure.Shared.DataStructures.Trees;

namespace Locations.Infrastructure.Shared.Services
{
    public class NearestLocationsFinderServiceV2 : INearestLocationsFinderService
    {
        private readonly ILocationsKdTree _locationsKdTree;

        public NearestLocationsFinderServiceV2(ILocationsKdTree locationsKdTree)
        {
            EnsureArg.IsNotNull(locationsKdTree, nameof(locationsKdTree));

            _locationsKdTree = locationsKdTree;
        }

        /// <summary>
        /// Find the K nearest neighbors.
        /// O(K logN)
        /// </summary>
        /// <param name="startingLocation"></param>
        /// <param name="maxDistance"></param>
        /// <param name="maxResults"></param>
        /// <returns></returns>
        public async Task<IEnumerable<LocationWithDistanceFromStartingPoint>> GetNearestLocations(StartingLocation startingLocation, int maxDistance, int maxResults)
        {
            EnsureArg.IsNotNull(startingLocation, nameof(startingLocation));
            EnsureArg.IsGte(maxDistance, 0, nameof(maxDistance));
            EnsureArg.IsGt(maxResults, 0, nameof(maxResults));

            var startLoc = new Location(startingLocation.Latitude, startingLocation.Longitude);

            var nearestNeighbors = _locationsKdTree.GetNearestNeighbors(
                startingLocation.Latitude,
                startingLocation.Longitude,
                maxResults);

            var res = nearestNeighbors
                    .Select(t =>
                    {
                        var loc = new Location(t.Item2, t.Item1[0], t.Item1[1]);
                        return new LocationWithDistanceFromStartingPoint(loc, loc.CalculateDistance(startLoc));
                    })
                    .Where(l => l.DistanceFromStartingPoint <= maxDistance) // Where: O(N)
                    .OrderBy(l => l.DistanceFromStartingPoint)
                    .ToList();

            return await Task.FromResult(res);
        }

        public async Task<IEnumerable<LocationWithDistanceFromStartingPoint>> GetNearestRadialLocations(StartingLocation startingLocation, int maxDistance, int maxResults)
        {
            var startLoc = new Location(startingLocation.Latitude, startingLocation.Longitude);

            var nearestNeighbors = _locationsKdTree.GetNearestNeighbors(
                startingLocation.Latitude,
                startingLocation.Longitude,
                maxResults);

            var res = nearestNeighbors
                .Select(t =>
                {
                    var loc = new Location(t.Item2, t.Item1[0], t.Item1[1]);
                    return new LocationWithDistanceFromStartingPoint(loc, loc.CalculateDistance(startLoc));
                })
                .Where(l => l.DistanceFromStartingPoint <= maxDistance)
                .OrderBy(l => l.DistanceFromStartingPoint)
                .Take(maxResults)
                .ToList();

            return await Task.FromResult(res);
        }

    }
}
