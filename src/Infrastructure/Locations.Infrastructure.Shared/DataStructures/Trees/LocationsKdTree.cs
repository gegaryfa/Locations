using System;
using System.Linq;

using EnsureThat;

using Locations.Core.Domain.Interfaces.Repositories;
using Locations.Core.Domain.Utilities;

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
}
