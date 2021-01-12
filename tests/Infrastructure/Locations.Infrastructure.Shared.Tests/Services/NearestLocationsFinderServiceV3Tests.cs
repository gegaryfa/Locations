using System;
using System.Linq;
using System.Threading.Tasks;

using FakeItEasy;

using FluentAssertions;

using Locations.Core.Application.Features.Locations.Queries.GetNearestLocations;
using Locations.Infrastructure.Shared.DataStructures.Trees;
using Locations.Infrastructure.Shared.Services;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locations.Infrastructure.Shared.Tests.Services
{
    [TestClass]
    public class NearestLocationsFinderServiceV3Tests
    {
        private ILocationsKdTree _locationsKdTree;
        private NearestLocationsFinderServiceV3 _nearestLocationsFinderService;

        [TestInitialize]
        public void Initialize()
        {
            _locationsKdTree = A.Fake<ILocationsKdTree>();

            _nearestLocationsFinderService = new NearestLocationsFinderServiceV3(_locationsKdTree);
        }

        [TestMethod]
        public void Constructor_WithNullForLocationsKdTree_ShouldThrowArgumentNullException()
        {
            // Arrange
            const string expectedNameOfParam = "locationsKdTree";

            // Act
            Action act = () => _ = new NearestLocationsFinderServiceV3(null);

            // Assert
            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be(expectedNameOfParam);
        }

        [TestMethod]
        public void GetNearestLocations_WithNullForLocation_ShouldThrowArgumentNullException()

        {
            // Arrange
            const string expectedNameOfParam = "startingLocation";

            // Act
            Func<Task> act = async () => await _nearestLocationsFinderService.GetNearestLocations(null, 1, 1);

            // Assert
            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be(expectedNameOfParam);

        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(-10)]
        public async Task GetNearestLocations_WithInvalidMaxDistance_ShouldThrowArgumentOutOfRangeException(int maxDistance)

        {
            // Arrange
            var startingLocation = new StartingLocation { Latitude = 0.1, Longitude = 0.2 };

            // Act
            Func<Task> act = async () =>
            {
                await _nearestLocationsFinderService.GetNearestLocations(startingLocation, maxDistance, 1);
            };

            // Assert
            await act.Should().ThrowAsync<ArgumentOutOfRangeException>();
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        public async Task GetNearestLocations_WithInvalidMaxResults_ShouldThrowArgumentOutOfRangeException(int maxResults)

        {
            // Arrange
            var startingLocation = new StartingLocation { Latitude = 0.1, Longitude = 0.2 };
            var maxDistance = 1;

            // Act
            Func<Task> act = async () =>
            {
                await _nearestLocationsFinderService.GetNearestLocations(startingLocation, maxDistance, maxResults);
            };

            // Assert
            await act.Should().ThrowAsync<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public async Task GetNearestLocations_WithValidInput_AllNeighborsWithinDistance()

        {
            // Arrange
            var startingLocation = new StartingLocation { Latitude = 50.91414, Longitude = 5.95549 };
            const int maxDistance = 100;
            const int maxResults = 2;

            var neighborsFromTree = new Tuple<double[], string>[]
            {
                new Tuple<double[], string>(new double[]
                {
                    50.91414,
                    5.95549
                }, "Test1"),
                new Tuple<double[], string>(new double[]
                {
                    50.9147729,
                    5.9555408
                }, "Test2")
            };

            A.CallTo(() =>
                _locationsKdTree.GetNearestRadialNeighbors(startingLocation.Latitude, startingLocation.Longitude,
                    maxDistance)).Returns(neighborsFromTree);

            // Act
            var actualNearestNeighbors = await _nearestLocationsFinderService.GetNearestLocations(startingLocation, maxDistance, maxResults);

            // Assert
            var locationWithDistanceFromStartingPoints = actualNearestNeighbors.ToList();
            locationWithDistanceFromStartingPoints.Count().Should().BeLessOrEqualTo(maxResults);

            var firstNeighbor = locationWithDistanceFromStartingPoints.First();
            firstNeighbor.Address.Should().Be("Test1");
            firstNeighbor.DistanceFromStartingPoint.Should().Be(0);


            var secondNeighbor = locationWithDistanceFromStartingPoints[1];
            secondNeighbor.Address.Should().Be("Test2");
            secondNeighbor.DistanceFromStartingPoint.Should().BeInRange(0, maxDistance);
        }

    }
}
