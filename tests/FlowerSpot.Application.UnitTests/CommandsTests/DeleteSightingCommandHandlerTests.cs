using AutoFixture;
using AutoMapper;
using FlowerSpot.Application.Contracts;
using FlowerSpot.Application.Features.Commands.DeleteSighting;
using FlowerSpot.Domain.Dtos;
using FlowerSpot.Domain.Entities;
using FlowerSpot.Domain.Resources;
using FlowerSpot.SharedKernel.Exceptions;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;

namespace FlowerSpot.Application.UnitTests.CommandsTests;

public class DeleteSightingCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<ISightingRepository> _sightingRepositoryMock = new();
    private readonly Mock<IFlowerSpotCache> _cacheServiceMock = new();

    private DeleteSightingCommandHandler _commandHandler;
    private readonly Fixture _fixture = new();
    private readonly IMapper _mapper;

    public DeleteSightingCommandHandlerTests()
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
        var request = _fixture.Create<DeleteSightingCommand>();

        _userRepositoryMock.Setup(x => x.GetSingleMatch(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync((User?)null);
        _commandHandler = new DeleteSightingCommandHandler(_userRepositoryMock.Object, _sightingRepositoryMock.Object, _cacheServiceMock.Object, _mapper);

        // Act
        var caughtException = await Assert.ThrowsAsync<NotFoundException> (() => _commandHandler.Handle(request, It.IsAny<CancellationToken>()));

        // Assert
        _userRepositoryMock.Verify(x => x.GetSingleMatch(It.IsAny<Expression<Func<User, bool>>>()), Times.Once);

        caughtException.Should().NotBeNull().And.BeOfType<NotFoundException>();
        caughtException.Message.Should().Be(string.Format(ExceptionMessages.UserNotFound, request.Username));
    }

    [Fact]
    public async void Handle_Fail_SightingNotFound()
    {
        // Arrange
        var request = _fixture.Create<DeleteSightingCommand>();
        var user = _fixture.Create<User>();

        _userRepositoryMock.Setup(x => x.GetSingleMatch(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(user);
        _sightingRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync((Sighting?)null);

        _commandHandler = new DeleteSightingCommandHandler(_userRepositoryMock.Object, _sightingRepositoryMock.Object, _cacheServiceMock.Object, _mapper);

        // Act
        var caughtException = await Assert.ThrowsAsync<NotFoundException> (() => _commandHandler.Handle(request, It.IsAny<CancellationToken>()));

        // Assert
        _userRepositoryMock.Verify(x => x.GetSingleMatch(It.IsAny<Expression<Func<User, bool>>>()), Times.Once);
        _sightingRepositoryMock.Verify(x => x.GetById(request.Id), Times.Once);

        caughtException.Should().NotBeNull().And.BeOfType<NotFoundException>();
        caughtException.Message.Should().Be(string.Format(ExceptionMessages.SightingNotFound, request.Id));
    }

    [Fact]
    public async void Handle_Fail_UserDoesNotHavePermission()
    {
        // Arrange
        var request = _fixture.Create<DeleteSightingCommand>();
        var user = _fixture.Create<User>();
        var sighting = _fixture.Build<Sighting>().With(x => x.UserId, 9999).Create();

        _userRepositoryMock.Setup(x => x.GetSingleMatch(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(user);
        _sightingRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(sighting);

        _commandHandler = new DeleteSightingCommandHandler(_userRepositoryMock.Object, _sightingRepositoryMock.Object, _cacheServiceMock.Object, _mapper);

        // Act
        var caughtException = await Assert.ThrowsAsync<UnauthorizedException> (() => _commandHandler.Handle(request, It.IsAny<CancellationToken>()));

        // Assert
        _userRepositoryMock.Verify(x => x.GetSingleMatch(It.IsAny<Expression<Func<User, bool>>>()), Times.Once);
        _sightingRepositoryMock.Verify(x => x.GetById(request.Id), Times.Once);

        caughtException.Should().NotBeNull().And.BeOfType<UnauthorizedException>();
        caughtException.Message.Should().Be(string.Format(ExceptionMessages.MissingPermission, user.Username));
    }

    [Fact]
    public async void Handle_Success()
    {
        // Arrange
        var request = _fixture.Create<DeleteSightingCommand>();
        var user = _fixture.Create<User>();
        var sighting = _fixture.Build<Sighting>().With(x => x.UserId, user.Id).Create();

        _userRepositoryMock.Setup(x => x.GetSingleMatch(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(user);
        _sightingRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(sighting);
        _sightingRepositoryMock.Setup(x => x.Delete(It.IsAny<Sighting>()));
        _sightingRepositoryMock.Setup(x => x.GetAllSightingsAsync()).ReturnsAsync(new List<Sighting>() { sighting });
        _cacheServiceMock.Setup(x => x.AddSightings(It.IsAny<List<SightingDto>>()));

        _commandHandler = new DeleteSightingCommandHandler(_userRepositoryMock.Object, _sightingRepositoryMock.Object, _cacheServiceMock.Object, _mapper);

        // Act
        var caughtException = await _commandHandler.Handle(request, It.IsAny<CancellationToken>());

        // Assert
        _userRepositoryMock.Verify(x => x.GetSingleMatch(It.IsAny<Expression<Func<User, bool>>>()), Times.Once);
        _sightingRepositoryMock.Verify(x => x.GetById(request.Id), Times.Once);
        _sightingRepositoryMock.Verify(x => x.Delete(sighting), Times.Once);
        _sightingRepositoryMock.Verify(x => x.GetAllSightingsAsync(), Times.Once);
        _cacheServiceMock.Verify(x => x.AddSightings(It.IsAny<List<SightingDto>>()), Times.Once);
    }

    #endregion Handle
}