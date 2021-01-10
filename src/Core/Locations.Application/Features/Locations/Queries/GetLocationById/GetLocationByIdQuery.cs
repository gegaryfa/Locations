using System.Threading;
using System.Threading.Tasks;

using Locations.Core.Application.Exceptions;
using Locations.Core.Application.Wrappers;
using Locations.Core.Domain.Entities;
using Locations.Core.Domain.Interfaces.Repositories;

using MediatR;

namespace Locations.Core.Application.Features.Locations.Queries.GetLocationById
{
    public class GetLocationByIdQuery : IRequest<Response<Location>>
    {
        public int Id { get; set; }
        public class GetLocationByIdQueryHandler : IRequestHandler<GetLocationByIdQuery, Response<Location>>
        {
            private readonly ILocationsRepositoryAsync _locationsRepository;
            public GetLocationByIdQueryHandler(ILocationsRepositoryAsync locationsRepository)
            {
                _locationsRepository = locationsRepository;
            }
            public async Task<Response<Location>> Handle(GetLocationByIdQuery query, CancellationToken cancellationToken)
            {
                var location = await _locationsRepository.GetByIdAsync(query.Id);
                if (location == null) throw new ApiException($"Location Not Found.");
                return new Response<Location>(location);
            }
        }
    }
}
