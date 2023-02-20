using AutoFixture;
using AutoMapper;
using FlowerSpot.Application.Contracts;
using FlowerSpot.Application.Features.Commands.AddFlower;
using FlowerSpot.Domain.Entities;
using FlowerSpot.Domain.Resources;
using FlowerSpot.SharedKernel.Contracts;
using FlowerSpot.SharedKernel.Exceptions;
using FluentAssertions;
using MediatR;
using Moq;
using System.Linq.Expressions;

namespace FlowerSpot.Application.UnitTests.CommandsTests;

public class AddFlowerCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IProcessingQueue<Flower>> _flowerQueueMock = new();

    private AddFlowerCommandHandler _commandHandler;
    private readonly Fixture _fixture = new();
    private readonly IMapper _mapper;

    public AddFlowerCommandHandlerTests()
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
        var request = _fixture.Create<AddFlowerCommand>();

        _userRepositoryMock.Setup(x => x.GetSingleMatch(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync((User?)null);
        _commandHandler = new AddFlowerCommandHandler(_userRepositoryMock.Object, _flowerQueueMock.Object, _mapper);

        // Act
        var caughtException = await Assert.ThrowsAsync<NotFoundException> (() => _commandHandler.Handle(request, It.IsAny<CancellationToken>()));

        // Assert
        _userRepositoryMock.Verify(x => x.GetSingleMatch(It.IsAny<Expression<Func<User, bool>>>()), Times.Once);
        _flowerQueueMock.Verify(x => x.Enqueue(It.IsAny<Flower>()), Times.Never);

        caughtException.Should().NotBeNull().And.BeOfType<NotFoundException>();
        caughtException.Message.Should().Be(string.Format(ExceptionMessages.UserNotFound, request.Username));
    }

    [Fact]
    public async void Handle_Success()
    {
        // Arrange
        var request = _fixture.Create<AddFlowerCommand>();
        var user = _fixture.Create<User>();

        _userRepositoryMock.Setup(x => x.GetSingleMatch(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(user);
        _flowerQueueMock.Setup(x => x.Enqueue(It.IsAny<Flower>()));

        _commandHandler = new AddFlowerCommandHandler(_userRepositoryMock.Object, _flowerQueueMock.Object, _mapper);

        // Act
        await _commandHandler.Handle(request, It.IsAny<CancellationToken>());

        // Assert
        _userRepositoryMock.Verify(x => x.GetSingleMatch(It.IsAny<Expression<Func<User, bool>>>()), Times.Once);
        _flowerQueueMock.Verify(x => x.Enqueue(It.IsAny<Flower>()), Times.Once);
    }

    #endregion Handle
}