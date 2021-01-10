using Locations.Core.Domain.Entities;

namespace Locations.Core.Application.Models
{
    public class LocationWithDistanceFromStartingPoint
    {
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double DistanceFromStartingPoint { get; set; }

        public LocationWithDistanceFromStartingPoint(Location location, double distanceFromStartingPoint)
        {
            Address = location.Address;
            Latitude = location.Latitude;
            Longitude = location.Longitude;
            DistanceFromStartingPoint = distanceFromStartingPoint;
        }
    }
}