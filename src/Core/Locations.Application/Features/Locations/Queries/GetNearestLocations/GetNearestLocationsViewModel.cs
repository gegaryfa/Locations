namespace Locations.Core.Application.Features.Locations.Queries.GetNearestLocations
{
    public class GetNearestLocationsViewModel
    {
        public string Address { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public double DistanceFromStartingPoint { get; set; }
    }
}
