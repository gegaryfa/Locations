
using FluentAssertions;

using Locations.Core.Domain.Entities;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locations.Core.Domain.Tests.Entities
{
    [TestClass]
    public class LocationTests
    {

        [DataTestMethod]
        [DataRow(50.91414, 5.95549, 50.91414, 5.95549, 0)]
        [DataRow(50.91414, 5.95549, 50.9147729, 5.9555408, 70.46188311452038)]
        [DataRow(50.91414, 5.95549, 50.9152271, 5.9558572, 123.58479049824584)]
        public void CalculateDistance_ReturnsCorrectDistanceFromLocationToLocation(double point1Lat, double point1Long, double point2Lat, double point2Long, double expectedDistanceBetweenPoints)
        {
            // Arrange
            var location1 = new Location("test1", point1Lat, point1Long);
            var location2 = new Location("test2", point2Lat, point2Long);

            // Act
            var actualDistance = location1.CalculateDistance(location2);

            // Assert
            actualDistance.Should().Be(expectedDistanceBetweenPoints);
        }

    }
}
