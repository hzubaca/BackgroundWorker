using FlowerSpot.Application.Features.Commands.AddFlower;
using FlowerSpot.Application.Features.Queries.GetFlowers;
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
    public class FlowersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FlowersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // It is assumed that the UI side will implement infinit scrol functionality, hence the query will return 20 latest flowers at the time (sorted by date modified)
        // If last loaded dateTime of a flower is in the Query of request, request will return next 20 rows or remaining rows added before it
        [AllowAnonymous]
        [HttpGet(Name = "GetFlowersPage")]
        public async Task<ActionResult<GetFlowersResponse>> GetFlowersPage([FromQuery] GetFlowersQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AddFlowerRequest request)
        {
            var username = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(username))
            {
                throw new UnauthorizedException(ExceptionMessages.UsernameClaimNotFound);
            }

            var command = new AddFlowerCommand(request.Name, request.ImageRef, request.Description, username);
            await _mediator.Send(command);

            return Accepted();
        }
    }
}