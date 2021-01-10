using System.Threading.Tasks;

using Locations.Core.Application.Features.Locations.Commands.CreateLocation;
using Locations.Core.Application.Features.Locations.Commands.DeleteLocationById;
using Locations.Core.Application.Features.Locations.Commands.UpdateLocation;
using Locations.Core.Application.Features.Locations.Queries.GetAllLocations;
using Locations.Core.Application.Features.Locations.Queries.GetLocationById;
using Locations.Core.Application.Features.Locations.Queries.GetNearestLocations;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Locations.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class LocationController : BaseApiController
    {
        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllLocationsParameters filter)
        {

            return Ok(await Mediator.Send(new GetAllLocationsQuery() { }));
        }

        // GET: api/<controller>
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

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetLocationByIdQuery { Id = id }));
        }


        // POST api/<controller>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(CreateLocationCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, UpdateLocationCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteLocationByIdCommand { Id = id }));
        }
    }
}
