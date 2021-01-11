using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using EnsureThat;

using Locations.Core.Application.Wrappers;
using Locations.Core.Domain.Interfaces.Repositories;

using MediatR;

namespace Locations.Core.Application.Features.Locations.Queries.GetAllLocations
{
    public class GetAllLocationsQuery : IRequest<PagedResponse<IEnumerable<GetAllLocationsViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class GetAllLocationsQueryHandler : IRequestHandler<GetAllLocationsQuery, PagedResponse<IEnumerable<GetAllLocationsViewModel>>>
    {
        private readonly ILocationsRepositoryAsync _locationsRepository;
        private readonly IMapper _mapper;
        public GetAllLocationsQueryHandler(ILocationsRepositoryAsync locationsRepository, IMapper mapper)
        {
            EnsureArg.IsNotNull(locationsRepository, nameof(locationsRepository));
            EnsureArg.IsNotNull(mapper, nameof(mapper));

            _locationsRepository = locationsRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<GetAllLocationsViewModel>>> Handle(GetAllLocationsQuery request, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotNull(request, nameof(request));

            var locations = await _locationsRepository.GetPagedResponseAsync(request.PageNumber, request.PageSize);
            var locationViewModel = _mapper.Map<IEnumerable<GetAllLocationsViewModel>>(locations);
            return new PagedResponse<IEnumerable<GetAllLocationsViewModel>>(locationViewModel, request.PageNumber, request.PageSize);
        }
    }
}
