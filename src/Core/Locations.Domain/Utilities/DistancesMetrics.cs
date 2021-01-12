using System;

namespace Locations.Core.Domain.Utilities
{
    /// <summary>
    /// This class provides methods to calculate the distance between two Geo points.
    /// It is used as a utilities class for the LocationsKdTree implementation.
    /// </summary>
    public static class DistanceMetrics
    {
        private const double MilesToKilometers = 1609.344;

        // Radians over degrees = (Math.PI / 180.0);
        private const double ToRadians = Math.PI / 180;

        /// <summary>
        /// Calculates the distance between two points (Geo coordinates)
        /// Spherical Law of Cosines formula to calculate 
        /// great-circle (orthodromic) distance on Earth.
        /// Other distance metrics: https://stackoverflow.com/a/27883916/5126274
        /// </summary>
        public static Func<double[], double[], double> CalculateDistance = (point1, point2) =>
        {
            var point1Latitude = point1[0];
            var point2Latitude = point2[0];

            var point1Longitude = point1[1];
            var point2Longitude = point2[1];

            // Convert degrees to radians
            var rlat1 = point1Latitude * ToRadians;
            var rlat2 = point2Latitude * ToRadians;
            var rlon1 = point1Longitude * ToRadians;
            var rlon2 = point2Longitude * ToRadians;


            var theta = point1Longitude - point2Longitude;
            var rtheta = theta * ToRadians;

            var dist = Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            //Convert miles to km
            var distanceInKm = dist * MilesToKilometers;
            return distanceInKm;
        };
    }
}
