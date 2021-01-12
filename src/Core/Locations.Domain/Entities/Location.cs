using Locations.Core.Domain.Utilities;

namespace Locations.Core.Domain.Entities
{
    public class Location
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Location(string address, double latitude, double longitude)
        {
            Address = address;
            Latitude = latitude;
            Longitude = longitude;
        }

        public Location(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        /// <summary>
        /// Calculates the distance between this location and another one, in meters.
        /// </summary>
        public double CalculateDistance(Location location)
        {
            var point1 = new[]
            {
                this.Latitude,
                this.Longitude
            };

            var point2 = new[]
            {
                location.Latitude,
                location.Longitude
            };

            return DistanceMetrics.CalculateDistance(point1, point2);
        }
    }
}
