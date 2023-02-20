using AutoFixture;
using AutoMapper;
using FlowerSpot.Application.Contracts;
using FlowerSpot.Application.Features.Commands.AddFlower;
using FlowerSpot.Application.Features.Commands.AddSighting;
using FlowerSpot.Domain.Dtos;
using FlowerSpot.Domain.Entities;
using FlowerSpot.Domain.Resources;
using FlowerSpot.SharedKernel.Contracts;
using FlowerSpot.SharedKernel.Exceptions;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;

namespace FlowerSpot.Application.UnitTests.CommandsTests;

public class AddSightingCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IProcessingQueue<Sighting>> _sightingQueueMock = new();
    private readonly Mock<IQuoteService> _quoteServiceMock = new();

    private AddSightingCommandHandler _commandHandler;
    private readonly Fixture _fixture = new();
    private readonly IMapper _mapper;

    public AddSightingCommandHandlerTests()
    {
        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var mockMapper = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile()));
        _mapper = mockMapper.CreateMapper();
    }

    #region Handle

    [Fact]
    public async void Handle_Fail_UserNotFound()
    {
        // Arrange
        var request = _fixture.Create<AddSightingCommand>();

        _userRepositoryMock.Setup(x => x.GetSingleMatch(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync((User?)null);
        _commandHandler = new AddSightingCommandHandler(_userRepositoryMock.Object, _sightingQueueMock.Object, _quoteServiceMock.Object, _mapper);

        // Act
        var caughtException = await Assert.ThrowsAsync<NotFoundException> (() => _commandHandler.Handle(request, It.IsAny<CancellationToken>()));

        // Assert
        _userRepositoryMock.Verify(x => x.GetSingleMatch(It.IsAny<Expression<Func<User, bool>>>()), Times.Once);
        _quoteServiceMock.Verify(x => x.GetQuoteOfTheDay(), Times.Never);
        _sightingQueueMock.Verify(x => x.Enqueue(It.IsAny<Sighting>()), Times.Never);

        caughtException.Should().NotBeNull().And.BeOfType<NotFoundException>();
        caughtException.Message.Should().Be(string.Format(ExceptionMessages.UserNotFound, request.Username));
    }

    [Fact]
    public async void Handle_Success()
    {
        // Arrange
        var request = _fixture.Create<AddSightingCommand>();
        var user = _fixture.Create<User>();
        var quote = _fixture.Create<QuoteDto>();

        _userRepositoryMock.Setup(x => x.GetSingleMatch(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(user);
        _quoteServiceMock.Setup(x => x.GetQuoteOfTheDay());
        _sightingQueueMock.Setup(x => x.Enqueue(It.IsAny<Sighting>()));

        _commandHandler = new AddSightingCommandHandler(_userRepositoryMock.Object, _sightingQueueMock.Object, _quoteServiceMock.Object, _mapper);

        // Act
        await _commandHandler.Handle(request, It.IsAny<CancellationToken>());

        // Assert
        _userRepositoryMock.Verify(x => x.GetSingleMatch(It.IsAny<Expression<Func<User, bool>>>()), Times.Once);
        _quoteServiceMock.Verify(x => x.GetQuoteOfTheDay(), Times.Once);
        _sightingQueueMock.Verify(x => x.Enqueue(It.IsAny<Sighting>()), Times.Once);
    }

    #endregion Handle
}