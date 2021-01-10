using System.Threading;
using System.Threading.Tasks;

using Locations.Core.Application.Exceptions;
using Locations.Core.Application.Wrappers;
using Locations.Core.Domain.Interfaces.Repositories;

using MediatR;

namespace Locations.Core.Application.Features.Locations.Commands.DeleteLocationById
{
    public class DeleteLocationByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteLocationByIdCommandHandler : IRequestHandler<DeleteLocationByIdCommand, Response<int>>
        {
            private readonly ILocationsRepositoryAsync _locationsRepository;
            public DeleteLocationByIdCommandHandler(ILocationsRepositoryAsync locationsRepository)
            {
                _locationsRepository = locationsRepository;
            }
            public async Task<Response<int>> Handle(DeleteLocationByIdCommand command, CancellationToken cancellationToken)
            {
                var location = await _locationsRepository.GetByIdAsync(command.Id);
                if (location == null) throw new ApiException($"Location Not Found.");
                await _locationsRepository.DeleteAsync(location);
                return new Response<int>(location.Id);
            }
        }
    }
}
