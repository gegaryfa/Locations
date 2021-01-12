using FluentAssertions;

using Locations.Core.Domain.Utilities;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locations.Core.Domain.Tests.Utilities
{
    [TestClass]
    public class DistanceMetricsTests
    {
        [DataTestMethod]
        [DataRow(50.91414, 5.95549, 50.91414, 5.95549, 0)]
        [DataRow(50.91414, 5.95549, 50.9147729, 5.9555408, 70.46188311452038)]
        [DataRow(50.91414, 5.95549, 50.9152271, 5.9558572, 123.58479049824584)]
        public void CalculateDistance_ReturnsCorrectDistanceBetweenTwoPoints(double point1Lat, double point1Long, double point2Lat, double point2Long, double expectedDistanceBetweenPoints)
        {
            // Arrange
            var point1 = new double[] { point1Lat, point1Long };
            var point2 = new double[] { point2Lat, point2Long };

            // Act
            var actualDistance = DistanceMetrics.CalculateDistance(point1, point2);

            // Assert
            actualDistance.Should().Be(expectedDistanceBetweenPoints);
        }
    }
}
