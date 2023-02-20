using FlowerSpot.Application.Features.Commands.AddSighting;
using FlowerSpot.Application.Features.Commands.DeleteSighting;
using FlowerSpot.Application.Features.Commands.LikeSighting;
using FlowerSpot.Application.Features.Commands.UnlikeSighting;
using FlowerSpot.Application.Features.Queries.GetSighting;
using FlowerSpot.Domain.Resources;
using FlowerSpot.SharedKernel.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FlowerSpot.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LikesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LikesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{sightingId}")]
        public async Task<ActionResult> Like([FromRoute] int sightingId)
        {
            var username = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(username))
            {
                throw new UnauthorizedException(ExceptionMessages.UsernameClaimNotFound);
            }

            var command = new LikeSightingCommand(sightingId, username);
            await _mediator.Send(command);

            return Accepted();
        }

        [HttpDelete("{sightingId}")]
        public async Task<ActionResult> Unlike([FromRoute] int sightingId)
        {
            var username = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(username))
            {
                throw new UnauthorizedException(ExceptionMessages.UsernameClaimNotFound);
            }

            var command = new UnlikeSightingCommand(sightingId, username);
            await _mediator.Send(command);

            return Ok();
        }
    }
}
