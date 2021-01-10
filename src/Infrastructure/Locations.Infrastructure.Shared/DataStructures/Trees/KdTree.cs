using System;
using System.Linq;

using Locations.Core.Domain.Interfaces.Repositories;

using Microsoft.Extensions.DependencyInjection;

using Supercluster.KDTree;

namespace Locations.Infrastructure.Shared.DataStructures.Trees
{
    public interface ILocationsKdTree
    {
        Tuple<double[], string>[] GetNearestNeighbors(double latitude, double longitude, int maxNeighbors);
    }

    public class LocationsKdTree : ILocationsKdTree
    {
        private KDTree<double, string> _kdTree;
        private readonly IServiceProvider _serviceProvider;

        public LocationsKdTree(IServiceProvider serviceProvider)
        {
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
    }
}
