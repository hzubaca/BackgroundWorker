using AutoFixture;
using FlowerSpot.Domain.Entities;
using FlowerSpot.Infrastructure.Persistence;
using MockQueryable.Moq;
using Moq;

namespace FlowerSpot.Infrastructure.UnitTests.RepositoriesTests;
public class MockHelper
{
    private readonly Fixture _fixture = new();

    public Mock<FlowerSpotContext> MockEntityModel()
    {
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var dbContextMock = new Mock<FlowerSpotContext>();

        // Lists to be mocked
        var flowers = _fixture.Build<Flower>().CreateMany(25);

        // Convert lists to DbSet
        var mockedFlowers = flowers.AsQueryable().BuildMockDbSet();

        // Setup mock
        dbContextMock.Setup(db => db.Flowers).Returns(mockedFlowers.Object);

        return dbContextMock;
    }
}
