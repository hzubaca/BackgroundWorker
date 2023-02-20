using AutoFixture;
using FlowerSpot.Infrastructure.Persistence;
using FlowerSpot.Infrastructure.Repositories;
using FlowerSpot.Infrastructure.UnitTests.RepositoriesTests;
using FluentAssertions;
using Moq;

namespace FlowerSpot.Application.UnitTests.CommandsTests;

public class FlowerRepositoryTests
{
    private readonly FlowerRepository _flowerRepository;
    private readonly Mock<FlowerSpotContext> _context;
    private readonly Fixture _fixture = new();

    public FlowerRepositoryTests()
    {
        _context = new MockHelper().MockEntityModel();
        _flowerRepository = new FlowerRepository(_context.Object);
    }

    #region GetPage

    [Fact]
    public async void Handle_Success_FirstPageLoaded()
    {
        // Arrange

        // Act
        var result = await _flowerRepository.GetPage(null);

        // Assert
        result.Should().NotBeNullOrEmpty().And.HaveCount(20);
    }

    [Fact]
    public async void Handle_Success_SecondPageLoaded()
    {
        // Arrange
        var expectedPageItems = _context.Object.Flowers.OrderByDescending(f => f.DateModified).Skip(19);
        var date = expectedPageItems.First().DateModified;

        // Act
        var result = await _flowerRepository.GetPage(date);

        // Assert
        result.Should().NotBeNullOrEmpty().And.HaveCount(5).And.OnlyContain(x => x.DateModified < date);
    }

    #endregion GetPage
}