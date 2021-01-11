using System;

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
        /// Some discussion around this: https://stackoverflow.com/questions/6366408/calculating-distance-between-two-latitude-and-longitude-geocoordinates/6366657
        /// </summary>
        public double CalculateDistance(Location location)
        {
            // Convert degrees to radians
            // radians over degrees = (Math.PI / 180.0);
            var rlat1 = Math.PI * Latitude / 180;
            var rlat2 = Math.PI * location.Latitude / 180;
            var rlon1 = Math.PI * Longitude / 180;
            var rlon2 = Math.PI * location.Longitude / 180;

            //
            var theta = Longitude - location.Longitude;
            var rtheta = Math.PI * theta / 180;

            var dist = Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            //Convert miles to km
            return dist * 1609.344;
        }

    }
}
