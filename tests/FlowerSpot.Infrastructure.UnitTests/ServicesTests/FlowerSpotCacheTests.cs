using AutoFixture;
using FlowerSpot.Domain.Common;
using FlowerSpot.Domain.Dtos;
using FlowerSpot.Domain.Entities;
using FlowerSpot.Infrastructure.Services;
using FlowerSpot.SharedKernel.Contracts;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerSpot.Infrastructure.UnitTests.ServicesTests;
public class FlowerSpotCacheTests
{
    private readonly Mock<IDistributedCacheService> _distributedCacheServiceMock = new();
    private FlowerSpotCache _flowerSpotCache;
    private readonly Fixture _fixture = new();

    public FlowerSpotCacheTests()
    {
    }

    #region GetSightings

    [Fact]
    public async void GetSightings_Success_Empty()
    {
        // Arrange
        _distributedCacheServiceMock.Setup(x => x.Get<List<SightingDto>>(It.IsAny<string>())).ReturnsAsync(new List<SightingDto>());
        _flowerSpotCache = new FlowerSpotCache(_distributedCacheServiceMock.Object);

        // Act
        var result = await _flowerSpotCache.GetSightings();

        // Assert
        _distributedCacheServiceMock.Verify(x => x.Get<List<SightingDto>>(CacheKeys.SIGHTINGS_CACHE_KEY), Times.Once);

        result.Should().BeEmpty();
    }

    [Fact]
    public async void GetSightings_Success()
    {
        // Arrange
        var sightings = _fixture.CreateMany<SightingDto>().ToList();

        _distributedCacheServiceMock.Setup(x => x.Get<List<SightingDto>>(It.IsAny<string>())).ReturnsAsync(sightings);
        _flowerSpotCache = new FlowerSpotCache(_distributedCacheServiceMock.Object);

        // Act
        var result = await _flowerSpotCache.GetSightings();

        // Assert
        _distributedCacheServiceMock.Verify(x => x.Get<List<SightingDto>>(CacheKeys.SIGHTINGS_CACHE_KEY), Times.Once);

        result.Should().NotBeNullOrEmpty().And.AllBeOfType<SightingDto>();
    }

    #endregion GetSightings

    #region GetSightingLikes

    [Fact]
    public async void GetSightingLikes_Success_Empty()
    {
        // Arrange
        var sightingId = _fixture.Create<int>();

        _distributedCacheServiceMock.Setup(x => x.Get<List<UserSightingLike>>(It.IsAny<string>())).ReturnsAsync(new List<UserSightingLike>());
        _flowerSpotCache = new FlowerSpotCache(_distributedCacheServiceMock.Object);

        // Act
        var result = await _flowerSpotCache.GetSightingLikes(sightingId);

        // Assert
        _distributedCacheServiceMock.Verify(x => x.Get<List<UserSightingLike>>(CacheKeys.SIGHTINGS_LIKES_CACHE_KEY), Times.Once);

        result.Should().BeEmpty();
    }

    [Fact]
    public async void GetSightingLikes_Success_Empty_NoMatches()
    {
        // Arrange
        var sightings = _fixture.CreateMany<UserSightingLike>().ToList();
        var sightingId = _fixture.Create<int>();

        _distributedCacheServiceMock.Setup(x => x.Get<List<UserSightingLike>>(It.IsAny<string>())).ReturnsAsync(sightings);
        _flowerSpotCache = new FlowerSpotCache(_distributedCacheServiceMock.Object);

        // Act
        var result = await _flowerSpotCache.GetSightingLikes(sightingId);

        // Assert
        _distributedCacheServiceMock.Verify(x => x.Get<List<UserSightingLike>>(CacheKeys.SIGHTINGS_LIKES_CACHE_KEY), Times.Once);

        result.Should().BeEmpty();
    }

    [Fact]
    public async void GetSightingLikes_Success()
    {
        // Arrange
        var sightings = _fixture.CreateMany<UserSightingLike>().ToList();
        var sightingId = sightings.First().SightingId;

        _distributedCacheServiceMock.Setup(x => x.Get<List<UserSightingLike>>(It.IsAny<string>())).ReturnsAsync(sightings);
        _flowerSpotCache = new FlowerSpotCache(_distributedCacheServiceMock.Object);

        // Act
        var result = await _flowerSpotCache.GetSightingLikes(sightingId);

        // Assert
        _distributedCacheServiceMock.Verify(x => x.Get<List<UserSightingLike>>(CacheKeys.SIGHTINGS_LIKES_CACHE_KEY), Times.Once);

        result.Should().NotBeNullOrEmpty().And.AllBeOfType<UserSightingLike>();
        result.TrueForAll(x => x.SightingId == sightingId);
    }

    #endregion GetSightingLikes
}