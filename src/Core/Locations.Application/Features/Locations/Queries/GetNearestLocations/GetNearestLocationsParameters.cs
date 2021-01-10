namespace Locations.Core.Application.Features.Locations.Queries.GetNearestLocations
{
    public class GetNearestLocationsParameters
    {
        public StartingLocation Location { get; set; }

        public int MaxDistance { get; set; }

        public int MaxResults { get; set; }
    }

    public class StartingLocation
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
