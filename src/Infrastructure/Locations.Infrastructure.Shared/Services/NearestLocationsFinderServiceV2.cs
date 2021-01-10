using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using KdTree;
using KdTree.Math;

using Locations.Core.Application.Features.Locations.Queries.GetNearestLocations;
using Locations.Core.Application.Interfaces.Services;
using Locations.Core.Application.Models;
using Locations.Core.Domain.Entities;
using Locations.Core.Domain.Interfaces.Repositories;

namespace Locations.Infrastructure.Shared.Services
{
    public class NearestLocationsFinderServiceV2 : INearestLocationsFinderService
    {
        private readonly ILocationsRepositoryAsync _locationsRepository;

        private KdTree<double, string> _kdTree;

        public NearestLocationsFinderServiceV2(ILocationsRepositoryAsync locationsRepository)
        {
            _locationsRepository = locationsRepository;
        }

        public async Task<IEnumerable<LocationWithDistanceFromStartingPoint>> GetNearestLocations(StartingLocation startingLocation, int maxDistance, int maxResults)
        {
            // In the case of an actual database with lots of records, we can use caching to save time when 
            // fetching al the records.
            var allLocations = await _locationsRepository.GetAllAsync();

            var startLoc = new Location(startingLocation.Latitude, startingLocation.Longitude);

            if (_kdTree == null)
            {
                _kdTree = new KdTree<double, string>(2, new DoubleMath());
                AddNodesToKdTree(allLocations);
            }

            var count = _kdTree.Count;

            var expectedNeighbors = allLocations
                .Select(l => new LocationWithDistanceFromStartingPoint(l, l.CalculateDistance(startLoc))) // Select: O(N)
                .Where(l => l.DistanceFromStartingPoint <= maxDistance) // Where: O(N)
                .OrderBy(l => l.DistanceFromStartingPoint) //O(M log M)
                .Take(maxResults); // Take: O(M)

            var actualNeighbors = _kdTree.GetNearestNeighbours(
                new[] { Math.Abs(startingLocation.Longitude), Math.Abs(startingLocation.Latitude) },
                maxResults);
            var treeActualNeighbors = actualNeighbors
                .Select(n => new Location(n.Value, n.Point[0], n.Point[1]))
                .Select(l =>
                    new LocationWithDistanceFromStartingPoint(l, l.CalculateDistance(startLoc))) // Select: O(K)
                                                                                                 //.Where(l => l.DistanceFromStartingPoint <= maxDistance) // Where: O(K)
                .OrderBy(l => l.DistanceFromStartingPoint)
                .Take(maxResults);



            return expectedNeighbors;
        }

        private void AddNodesToKdTree(IEnumerable<Location> allLocations)
        {
            foreach (var location in allLocations)
            {
                if (!_kdTree.Add(new[] { location.Longitude, location.Latitude }, location.Address))
                {
                    //throw new Exception("Failed to add node to tree");
                }

            }
            _kdTree.Balance();

        }
    }
}
