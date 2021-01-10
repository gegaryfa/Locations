using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using Locations.Core.Application.Interfaces.Services;

using MediatR;

namespace Locations.Core.Application.Features.Locations.Queries.GetNearestLocations
{
    public class GetNearestLocationsQuery : IRequest<IEnumerable<GetNearestLocationsViewModel>>
    {
        public StartingLocation StartingPoint { get; set; }

        public int MaxDistance { get; set; }

        public int MaxResults { get; set; }
    }

    public class GetAllLocationsQueryHandler : IRequestHandler<GetNearestLocationsQuery, IEnumerable<GetNearestLocationsViewModel>>
    {
        private readonly INearestLocationsFinderService _nearestLocationsFinderService;
        private readonly IMapper _mapper;
        public GetAllLocationsQueryHandler(INearestLocationsFinderService nearestLocationsFinderService, IMapper mapper)
        {
            _nearestLocationsFinderService = nearestLocationsFinderService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetNearestLocationsViewModel>> Handle(GetNearestLocationsQuery request, CancellationToken cancellationToken)
        {
            var locations = await _nearestLocationsFinderService.GetNearestLocations(request.StartingPoint, request.MaxDistance, request.MaxResults);
            var locationViewModel = _mapper.Map<IEnumerable<GetNearestLocationsViewModel>>(locations);
            return new List<GetNearestLocationsViewModel>(locationViewModel);
        }
    }
}
