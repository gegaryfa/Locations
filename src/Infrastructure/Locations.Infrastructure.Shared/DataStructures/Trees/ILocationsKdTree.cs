using System;

namespace Locations.Infrastructure.Shared.DataStructures.Trees
{
    public interface ILocationsKdTree
    {
        Tuple<double[], string>[] GetNearestNeighbors(double latitude, double longitude, int maxNeighbors);

        Tuple<double[], string>[] GetNearestRadialNeighbors(double latitude, double longitude, int maxDistance);
    }
}