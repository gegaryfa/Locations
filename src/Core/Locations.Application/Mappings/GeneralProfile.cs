using AutoMapper;

using Locations.Core.Application.Features.Locations.Commands.CreateLocation;
using Locations.Core.Application.Features.Locations.Queries.GetAllLocations;
using Locations.Core.Application.Features.Locations.Queries.GetNearestLocations;
using Locations.Core.Application.Models;
using Locations.Core.Domain.Entities;

namespace Locations.Core.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Location, GetAllLocationsViewModel>().ReverseMap();
            CreateMap<LocationWithDistanceFromStartingPoint, GetNearestLocationsViewModel>().ReverseMap();
            CreateMap<CreateLocationCommand, Location>();
        }
    }
}
