using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using OWP.CQRS;
using OWP.NewLeagueSetting.Query;
using OWP.NewLeagueSetting.Models;
using OWP.NewLeagueSetting.Data;
using Xunit;

namespace OWP.NewLeagueSetting.UnitTests;

public class QueryLeagueSettingDetailHandlerUnitTest
{
    [Fact]
    public async Task Handle_ShouldCallRepository_AndReturnLeagueSettingInfoSender()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var handler = new QueryLeagueSettingDetailHandler(repository);

        var request = new QueryLeagueSettingDetailRequest(123);

        var expectedResult = new LeagueSettingInfoSender
        {
            LeagueSettingId = 123,
            League = "Test League"
        };

        repository.QueryLeagueSettingDetail(123).Returns(expectedResult);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).QueryLeagueSettingDetail(123);
        Assert.Equal(expectedResult, result);
        Assert.Equal(123, result.LeagueSettingId);
    }

    [Fact]
    public async Task Handle_WithDifferentLeagueSettingId_ShouldCallRepositoryWithCorrectId()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var handler = new QueryLeagueSettingDetailHandler(repository);

        var request = new QueryLeagueSettingDetailRequest(456);

        var expectedResult = new LeagueSettingInfoSender
        {
            LeagueSettingId = 456,
            League = "Another League"
        };

        repository.QueryLeagueSettingDetail(456).Returns(expectedResult);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).QueryLeagueSettingDetail(456);
        Assert.Equal(expectedResult, result);
        Assert.Equal(456, result.LeagueSettingId);
    }

    [Fact]
    public async Task Handle_WithZeroLeagueSettingId_ShouldStillCallRepository()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var handler = new QueryLeagueSettingDetailHandler(repository);

        var request = new QueryLeagueSettingDetailRequest(0);

        var expectedResult = new LeagueSettingInfoSender
        {
            LeagueSettingId = 0,
            League = "Zero League"
        };

        repository.QueryLeagueSettingDetail(0).Returns(expectedResult);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).QueryLeagueSettingDetail(0);
        Assert.Equal(expectedResult, result);
        Assert.Equal(0, result.LeagueSettingId);
    }

    [Fact]
    public async Task Handle_WithNegativeLeagueSettingId_ShouldStillCallRepository()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var handler = new QueryLeagueSettingDetailHandler(repository);

        var request = new QueryLeagueSettingDetailRequest(-1);

        var expectedResult = new LeagueSettingInfoSender
        {
            LeagueSettingId = -1,
            League = "Negative League"
        };

        repository.QueryLeagueSettingDetail(-1).Returns(expectedResult);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).QueryLeagueSettingDetail(-1);
        Assert.Equal(expectedResult, result);
        Assert.Equal(-1, result.LeagueSettingId);
    }

    [Fact]
    public async Task Handle_WithLargeLeagueSettingId_ShouldCallRepositoryWithCorrectId()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var handler = new QueryLeagueSettingDetailHandler(repository);

        var largeId = int.MaxValue;
        var request = new QueryLeagueSettingDetailRequest(largeId);

        var expectedResult = new LeagueSettingInfoSender
        {
            LeagueSettingId = largeId,
            League = "Large ID League"
        };

        repository.QueryLeagueSettingDetail(largeId).Returns(expectedResult);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).QueryLeagueSettingDetail(largeId);
        Assert.Equal(expectedResult, result);
        Assert.Equal(largeId, result.LeagueSettingId);
    }

    [Fact]
    public async Task Handle_WithCancellationToken_ShouldPassTokenToRepository()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var handler = new QueryLeagueSettingDetailHandler(repository);

        var request = new QueryLeagueSettingDetailRequest(123);
        var cancellationToken = new CancellationToken(true); // Cancelled token

        var expectedResult = new LeagueSettingInfoSender
        {
            LeagueSettingId = 123,
            League = "Test League"
        };

        repository.QueryLeagueSettingDetail(123).Returns(expectedResult);

        // Act
        var result = await handler.Handle(request, cancellationToken);

        // Assert
        await repository.Received(1).QueryLeagueSettingDetail(123);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public async Task Handle_WithNullResult_ShouldReturnNull()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var handler = new QueryLeagueSettingDetailHandler(repository);

        var request = new QueryLeagueSettingDetailRequest(123);

        repository.QueryLeagueSettingDetail(123).Returns((LeagueSettingInfoSender)null!);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).QueryLeagueSettingDetail(123);
        Assert.Null(result);
    }
}
