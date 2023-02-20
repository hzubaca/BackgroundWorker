using AutoFixture;
using FlowerSpot.Application.Features.Commands.AddFlower;
using FlowerSpot.Application.Features.Queries.GetFlowers;
using FlowerSpot.Controllers;
using FlowerSpot.Domain.Resources;
using FlowerSpot.SharedKernel.Exceptions;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace FlowerSpot.Api.UnitTests.ControllersTests;

public class FlowersControllerTests
{
    private readonly Mock<IMediator> _mediator = new();
    private FlowersController _flowersController;
    private readonly Fixture _fixture = new();

    public FlowersControllerTests() { }

    #region GetAll

    [Fact]
    public async void GetAll_Success()
    {
        // Arrange
        _flowersController = new FlowersController(_mediator.Object);

        // Act
        var response = await _flowersController.GetFlowersPage(new GetFlowersQuery());

        // Assert
        response.Should().NotBeNull();
        response.Result.Should().NotBeNull().And.BeOfType<OkObjectResult>();
    }

    #endregion GetAll

    #region Post

    [Fact]
    public async void Post_Success()
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, "test_name"),
        }, "mock"));

        _flowersController = new FlowersController(_mediator.Object)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            }
        };

        var request = _fixture.Create<AddFlowerRequest>();

        // Act
        var response = await _flowersController.Post(request);

        // Assert
        response.Should().NotBeNull().And.BeOfType<AcceptedResult>();
    }

    [Fact]
    public async void Post_Fail_Unaothorized()
    {
        // Arrange
        var request = _fixture.Create<AddFlowerRequest>();
        _flowersController = new FlowersController(_mediator.Object);

        // Act
        var caughtException = await Assert.ThrowsAsync<UnauthorizedException>(() => _flowersController.Post(request));

        // Assert
        caughtException.Should().NotBeNull().And.BeOfType<UnauthorizedException>();
        caughtException.Message.Should().Be(ExceptionMessages.UsernameClaimNotFound);
    }

    #endregion Post
}