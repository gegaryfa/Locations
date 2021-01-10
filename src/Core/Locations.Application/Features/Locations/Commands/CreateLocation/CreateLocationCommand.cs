using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using Locations.Core.Application.Wrappers;
using Locations.Core.Domain.Entities;
using Locations.Core.Domain.Interfaces.Repositories;

using MediatR;

namespace Locations.Core.Application.Features.Locations.Commands.CreateLocation
{
    public partial class CreateLocationCommand : IRequest<Response<int>>
    {
        public string Address { get; set; }
    }
    public class CreateLocationCommandHandler : IRequestHandler<CreateLocationCommand, Response<int>>
    {
        private readonly ILocationsRepositoryAsync _locationsRepository;
        private readonly IMapper _mapper;
        public CreateLocationCommandHandler(ILocationsRepositoryAsync locationsRepository, IMapper mapper)
        {
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
