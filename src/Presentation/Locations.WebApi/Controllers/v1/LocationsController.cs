using System.Threading.Tasks;

using Locations.Core.Application.Features.Locations.Commands.CreateLocation;
using Locations.Core.Application.Features.Locations.Queries.GetAllLocations;
using Locations.Core.Application.Features.Locations.Queries.GetNearestLocations;

using Microsoft.AspNetCore.Mvc;


namespace Locations.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class LocationsController : BaseApiController
    {
        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllLocationsParameter filter)
        {
            return Ok(await Mediator.Send(new GetAllLocationsQuery() { PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
        }

        // GET: api/<controller>/getNearestLocations
        [HttpGet("getNearestLocations")]
        public async Task<IActionResult> Get([FromQuery] GetNearestLocationsParameters filter)
        {
            var query = new GetNearestLocationsQuery
            {
                StartingPoint = filter.Location,
                MaxDistance = filter.MaxDistance,
                MaxResults = filter.MaxResults
            };

            return Ok(await Mediator.Send(query));
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post(CreateLocationCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}
