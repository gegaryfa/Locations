using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using EnsureThat;

using Locations.Core.Application.Wrappers;
using Locations.Core.Domain.Entities;
using Locations.Core.Domain.Interfaces.Repositories;

using MediatR;

namespace Locations.Core.Application.Features.Locations.Commands.CreateLocation
{
    public class CreateLocationCommand : IRequest<Response<int>>
    {
    }

    public class CreateLocationCommandHandler : IRequestHandler<CreateLocationCommand, Response<int>>
    {
        private readonly ILocationsRepositoryAsync _locationsRepository;
        private readonly IMapper _mapper;
        public CreateLocationCommandHandler(ILocationsRepositoryAsync locationsRepository, IMapper mapper)
        {
            EnsureArg.IsNotNull(locationsRepository, nameof(locationsRepository));
            EnsureArg.IsNotNull(mapper, nameof(mapper));

            _locationsRepository = locationsRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateLocationCommand request, CancellationToken cancellationToken)
        {
            var location = _mapper.Map<Location>(request);
            await _locationsRepository.AddAsync(location);
            return new Response<int>(location.Id);
        }
    }
}
