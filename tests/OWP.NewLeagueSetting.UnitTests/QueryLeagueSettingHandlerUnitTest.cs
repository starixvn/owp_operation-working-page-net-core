using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NSubstitute;
using OWP.NewLeagueSetting.Query;
using OWP.NewLeagueSetting.Models;
using OWP.NewLeagueSetting.Data;
using Xunit;

namespace OWP.NewLeagueSetting.UnitTests;

public class QueryLeagueSettingHandlerUnitTest
{
    [Fact]
    public async Task Handle_ShouldCallRepository_AndReturnLeagueSettingInfoSenderCollection()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var handler = new QueryLeagueSettingHandler(repository);

        var request = new QueryLeagueSettingRequest
        {
            LeagueName = "Test League",
            IsEqualLeague = true
        };

        var expectedResult = new List<LeagueSettingInfoSender>
        {
            new LeagueSettingInfoSender { LeagueSettingId = 1, League = "Test League" },
            new LeagueSettingInfoSender { LeagueSettingId = 2, League = "Test League" }
        };

        repository.QueryLeagueSetting(request).Returns(expectedResult);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).QueryLeagueSetting(request);
        Assert.Equal(expectedResult, result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task Handle_WithEmptyRequest_ShouldStillCallRepository()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var handler = new QueryLeagueSettingHandler(repository);

        var request = new QueryLeagueSettingRequest();

        var expectedResult = new List<LeagueSettingInfoSender>();

        repository.QueryLeagueSetting(request).Returns(expectedResult);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).QueryLeagueSetting(request);
        Assert.Equal(expectedResult, result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_WithNullRequest_ShouldStillCallRepository()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var handler = new QueryLeagueSettingHandler(repository);

        QueryLeagueSettingRequest request = null!;

        var expectedResult = new List<LeagueSettingInfoSender>();

        repository.QueryLeagueSetting(request).Returns(expectedResult);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).QueryLeagueSetting(request);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public async Task Handle_WithSpecificLeagueName_ShouldCallRepositoryWithCorrectRequest()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var handler = new QueryLeagueSettingHandler(repository);

        var request = new QueryLeagueSettingRequest
        {
            LeagueName = "Premier League",
            IsEqualLeague = false
        };

        var expectedResult = new List<LeagueSettingInfoSender>
        {
            new LeagueSettingInfoSender { LeagueSettingId = 1, League = "Premier League" }
        };

        repository.QueryLeagueSetting(request).Returns(expectedResult);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).QueryLeagueSetting(request);
        Assert.Equal(expectedResult, result);
        Assert.Single(result);
    }

    [Fact]
    public async Task Handle_WithLargeResultSet_ShouldReturnAllResults()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var handler = new QueryLeagueSettingHandler(repository);

        var request = new QueryLeagueSettingRequest
        {
            LeagueName = "Multiple Leagues",
            IsEqualLeague = true
        };

        var expectedResult = new List<LeagueSettingInfoSender>();
        for (int i = 1; i <= 100; i++)
        {
            expectedResult.Add(new LeagueSettingInfoSender
            {
                LeagueSettingId = i,
                League = $"League {i}"
            });
        }

        repository.QueryLeagueSetting(request).Returns(expectedResult);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).QueryLeagueSetting(request);
        Assert.Equal(expectedResult, result);
        Assert.Equal(100, result.Count());
    }

    [Fact]
    public async Task Handle_WithCancellationToken_ShouldPassTokenToRepository()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var handler = new QueryLeagueSettingHandler(repository);

        var request = new QueryLeagueSettingRequest
        {
            LeagueName = "Test League",
            IsEqualLeague = true
        };

        var cancellationToken = new CancellationToken(true); // Cancelled token
        var expectedResult = new List<LeagueSettingInfoSender>();

        repository.QueryLeagueSetting(request).Returns(expectedResult);

        // Act
        var result = await handler.Handle(request, cancellationToken);

        // Assert
        await repository.Received(1).QueryLeagueSetting(request);
        Assert.Equal(expectedResult, result);
    }
}
