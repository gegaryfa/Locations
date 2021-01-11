using System;
using System.Linq;

using EnsureThat;

using Locations.Core.Domain.Interfaces.Repositories;

using Microsoft.Extensions.DependencyInjection;

using Supercluster.KDTree;

namespace Locations.Infrastructure.Shared.DataStructures.Trees
{
    public class LocationsKdTree : ILocationsKdTree
    {
        private KDTree<double, string> _kdTree;
        private readonly IServiceProvider _serviceProvider;

        public LocationsKdTree(IServiceProvider serviceProvider)
        {
            EnsureArg.IsNotNull(serviceProvider, nameof(serviceProvider));

            _serviceProvider = serviceProvider;

            PopulatedTree();
        }

        private void PopulatedTree()
        {
            using var scope = _serviceProvider.CreateScope();

            var locationsRepository = scope.ServiceProvider.GetRequiredService<ILocationsRepositoryAsync>();
            var allLocations = locationsRepository.GetAll();

            var points = allLocations.Select(l => new double[] { l.Latitude, l.Longitude }).ToArray();
            var nodes = allLocations.Select(l => l.Address).ToArray();

            _kdTree = new KDTree<double, string>(
                2,
                points,
                nodes,
                DistanceMetrics.CalculateDistance,
                double.MinValue,
                double.MaxValue);
        }

        public Tuple<double[], string>[] GetNearestNeighbors(double latitude, double longitude, int maxNeighbors)
        {
            var nearestNeighbors = _kdTree.NearestNeighbors(
                new double[]
                {
                    latitude,
                    longitude
                },
                maxNeighbors);

            return nearestNeighbors;
        }

        public Tuple<double[], string>[] GetNearestRadialNeighbors(double latitude, double longitude, int maxDistance)
        {
            var nearestNeighbors = _kdTree.RadialSearch(
                center: new double[]
                {
                    latitude,
                    longitude
                },
                radius: (double)maxDistance);

            return nearestNeighbors;
        }
    }

    /// <summary>
    /// This class provides methods to calculate the distance between two Geo points.
    /// It is used as a utilities class for the LocationsKdTree implementation.
    /// todo: move into domain?
    /// </summary>
    internal static class DistanceMetrics
    {
        /// <summary>
        /// Calculates the distance between two points (Geo coordinates)
        /// Spherical Law of Cosines formula to calculate 
        /// great-circle (orthodromic) distance on Earth.
        /// Other functions: https://stackoverflow.com/a/27883916/5126274
        /// </summary>
        public static Func<double[], double[], double> CalculateDistance = (point1, point2) =>
        {
            var point1Latitude = point1[0];
            var point2Latitude = point2[0];

            var point1Longitude = point1[1];
            var point2Longitude = point2[1];

            // Convert degrees to radians
            // radians over degrees = (Math.PI / 180.0);
            var rlat1 = Math.PI * point1Latitude / 180;
            var rlat2 = Math.PI * point2Latitude / 180;
            var rlon1 = Math.PI * point1Longitude / 180;
            var rlon2 = Math.PI * point2Longitude / 180;

            //
            var theta = point1Longitude - point2Longitude;
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
