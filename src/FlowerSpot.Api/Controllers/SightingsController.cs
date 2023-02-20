using FlowerSpot.Application.Features.Commands.AddSighting;
using FlowerSpot.Application.Features.Commands.DeleteSighting;
using FlowerSpot.Application.Features.Queries.GetSighting;
using FlowerSpot.Domain.Resources;
using FlowerSpot.SharedKernel.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FlowerSpot.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SightingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SightingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(Name = "GetSighting")]
        public async Task<ActionResult<GetSightingResponse>> Get([FromQuery] GetSightingQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AddSightingRequest request)
        {
            var username = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(username))
            {
                throw new UnauthorizedException(ExceptionMessages.UsernameClaimNotFound);
            }

            var command = new AddSightingCommand(request.Longitude, request.Latitude, username, request.FlowerId, request.ImageRef);
            await _mediator.Send(command);

            return Accepted();
        }

        [HttpDelete("{sightingId}")]
        public async Task<ActionResult> Delete([FromRoute] int sightingId)
        {
            var username = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(username))
            {
                throw new UnauthorizedException(ExceptionMessages.UsernameClaimNotFound);
            }

            var command = new DeleteSightingCommand(sightingId, username);
            await _mediator.Send(command);

            return Ok();
        }
    }
}