using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using Locations.Core.Domain.Interfaces.Repositories;

using MediatR;

namespace Locations.Core.Application.Features.Locations.Queries.GetAllLocations
{
    public class GetAllLocationsQuery : IRequest<IEnumerable<GetAllLocationsViewModel>>
    {
    }

    public class GetAllLocationsQueryHandler : IRequestHandler<GetAllLocationsQuery, IEnumerable<GetAllLocationsViewModel>>
    {
        private readonly ILocationsRepositoryAsync _locationsRepository;
        private readonly IMapper _mapper;
        public GetAllLocationsQueryHandler(ILocationsRepositoryAsync locationsRepository, IMapper mapper)
        {
            _locationsRepository = locationsRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAllLocationsViewModel>> Handle(GetAllLocationsQuery request, CancellationToken cancellationToken)
        {
            var locations = await _locationsRepository.GetAllAsync();
            var locationViewModel = _mapper.Map<IEnumerable<GetAllLocationsViewModel>>(locations);
            return new List<GetAllLocationsViewModel>(locationViewModel);
        }
    }
}
