using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Locations.Core.Application.Features.Locations.Queries.GetNearestLocations;
using Locations.Core.Application.Interfaces.Services;
using Locations.Core.Application.Models;
using Locations.Core.Domain.Entities;
using Locations.Core.Domain.Interfaces.Repositories;

using Supercluster.KDTree;

namespace Locations.Infrastructure.Shared.Services
{
    public class NearestLocationsFinderServiceV2 : INearestLocationsFinderService
    {
        private readonly ILocationsRepositoryAsync _locationsRepository;
        private KDTree<double, string> _kdTree;

        public NearestLocationsFinderServiceV2(ILocationsRepositoryAsync locationsRepository)
        {
            _locationsRepository = locationsRepository;
        }

        public async Task<IEnumerable<LocationWithDistanceFromStartingPoint>> GetNearestLocations(StartingLocation startingLocation, int maxDistance, int maxResults)
        {
            // In the case of an actual database with lots of records, we can use caching to save time when 
            // fetching al the records.
            var allLocations = await _locationsRepository.GetAllAsync();

            if (_kdTree is null)
            {
                PopulatedTree(allLocations);
            }

            var startLoc = new Location(startingLocation.Latitude, startingLocation.Longitude);

            // Time complexity is the same as sequential run, but this will improve the overall time.
            //var res = allLocations
            //    .Select(l => new LocationWithDistanceFromStartingPoint(l, l.CalculateDistance(startLoc))) // Select: O(N)
            //    .Where(l => l.DistanceFromStartingPoint <= maxDistance) // Where: O(N)
            //    .OrderBy(l => l.DistanceFromStartingPoint) //O(K log K)
            //    .Take(maxResults)// Take: O(M)
            //    .ToList();

            var treeNearest = _kdTree.NearestNeighbors(
                new double[]
                {
                    startingLocation.Latitude,
                    startingLocation.Longitude
                },
                maxResults);

            var res = treeNearest
                    .Select(t =>
                    {
                        var loc = new Location(t.Item2, t.Item1[0], t.Item1[1]);
                        return new LocationWithDistanceFromStartingPoint(loc, loc.CalculateDistance(startLoc));
                    })
                    .Where(l => l.DistanceFromStartingPoint <= maxDistance) // Where: O(N)
                    .OrderBy(l => l.DistanceFromStartingPoint)
                    .ToList();

            return res;
        }

        private void PopulatedTree(IReadOnlyList<Location> allLocations)
        {
            var points = allLocations.Select(l => new double[] { l.Latitude, l.Longitude }).ToArray();
            var nodes = allLocations.Select(l => l.Address).ToArray();

            _kdTree = new KDTree<double, string>(
                2,
                points,
                nodes,
                Utilities.CalculateDistance,
                double.MinValue,
                double.MaxValue);
        }

        internal static class Utilities
        {
            public static Func<double[], double[], double> CalculateDistance = (x, y) =>
            {
                // Convert degrees to radians
                // radians over degrees = (Math.PI / 180.0);
                var rlat1 = Math.PI * x[0] / 180;
                var rlat2 = Math.PI * y[0] / 180;
                var rlon1 = Math.PI * x[1] / 180;
                var rlon2 = Math.PI * y[1] / 180;

                //
                var theta = x[1] - y[1];
                var rtheta = Math.PI * theta / 180;

                var dist = Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Cos(rtheta);
                dist = Math.Acos(dist);
                dist = dist * 180 / Math.PI;
                dist = dist * 60 * 1.1515;

                //Convert miles to km
                var distanceInKm = dist * 1609.344;
                return distanceInKm;

            };
        }


    }
}
