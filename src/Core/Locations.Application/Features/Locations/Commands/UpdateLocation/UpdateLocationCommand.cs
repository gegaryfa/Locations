using System.Threading;
using System.Threading.Tasks;

using Locations.Core.Application.Exceptions;
using Locations.Core.Application.Wrappers;
using Locations.Core.Domain.Interfaces.Repositories;

using MediatR;

namespace Locations.Core.Application.Features.Locations.Commands.UpdateLocation
{
    public class UpdateLocationCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string Address { get; set; }

        public class UpdateLocationCommandHandler : IRequestHandler<UpdateLocationCommand, Response<int>>
        {
            private readonly ILocationsRepositoryAsync _locationsRepository;
            public UpdateLocationCommandHandler(ILocationsRepositoryAsync locationsRepository)
            {
                _locationsRepository = locationsRepository;
            }
            public async Task<Response<int>> Handle(UpdateLocationCommand command, CancellationToken cancellationToken)
            {
                var location = await _locationsRepository.GetByIdAsync(command.Id);

                if (location == null)
                {
                    throw new ApiException($"Location Not Found.");
                }
                else
                {
                    //product.Name = command.Name;
                    //product.Rate = command.Rate;
                    //product.Description = command.Description;
                    await _locationsRepository.UpdateAsync(location);
                    return new Response<int>(location.Id);
                }
            }
        }
    }
}
