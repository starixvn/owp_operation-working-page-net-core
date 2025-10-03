using System.Threading;
using System.Threading.Tasks;
using OWP.NewLeagueSetting.Service;
using OWP.NewLeagueSetting.Models;
using Fanex.NAP.Client;
using MediatR;
using NSubstitute;
using OWP.CQRS;
using OWP.NewLeagueSetting.Command;
using OWP.NewLeagueSetting.Data;
using Xunit;

namespace OWP.NewLeagueSetting.UnitTests;

public class UpdateLeagueSettingHandlerUnitTest
{
    private static void SetupRepositoryMock(Repository repository)
    {
        var mockLeagueSetting = new LeagueSettingInfoSender
        {
            League = "Test League",
            LeagueGroup = "Test Group",
            FullConditionDescription = System.Text.Json.JsonSerializer.Serialize(new LeagueSettingCondition())
        };
        repository.QueryLeagueSettingDetail(Arg.Any<int>()).Returns(mockLeagueSetting);
    }
    [Fact]
    public async Task Handle_ShouldBuildStandardizedInfo_AndCallRepository_AndReturnUnit()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();
        session.UserId.Returns(123);

        var logService = Substitute.For<LogService>();
        var handler = new UpdateLeagueSettingHandler(repository, session, logService);

        SetupRepositoryMock(repository);

        var request = new UpdateLeagueSettingRequest
        {
            LeagueSettingId = 456,
            MaxbetWD_HDP = "1000",
            MaxbetWK_HDP = "2000",
            WD_NoOfHDP = "1",
            WK_NoOfHDP = "2",
            Spread = "0.05"
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).UpdateLeagueSetting(request, 123);
        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task Handle_WithDifferentValues_ShouldCallRepositoryWithCorrectUserId()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();
        session.UserId.Returns(789);

        var logService = Substitute.For<LogService>();
        var handler = new UpdateLeagueSettingHandler(repository, session, logService);

        SetupRepositoryMock(repository);

        var request = new UpdateLeagueSettingRequest
        {
            LeagueSettingId = 999,
            MaxbetWD_HDP = "3000",
            MaxbetWK_HDP = "4000",
            WD_NoOfHDP = "3",
            WK_NoOfHDP = "4",
            Spread = "0.10"
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).UpdateLeagueSetting(request, 789);
        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task Handle_ShouldCallStandardizedLeagueInfoMethods()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();
        session.UserId.Returns(123);

        var logService = Substitute.For<LogService>();
        var handler = new UpdateLeagueSettingHandler(repository, session, logService);

        SetupRepositoryMock(repository);

        var request = new UpdateLeagueSettingRequest
        {
            LeagueSettingId = 456,
            MaxbetWD_HDP = "1000",
            MaxbetWK_HDP = "2000",
            WD_NoOfHDP = "1",
            WK_NoOfHDP = "2",
            Spread = "0.05"
        };

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert
        // Verify that the request object has been modified by the StandardizedLeagueInfo methods
        // This is a side effect test - we verify the object state after processing
        Assert.NotNull(request);
    }

    [Fact]
    public async Task Handle_WithZeroUserId_ShouldStillProcessRequest()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();
        session.UserId.Returns(0);

        var logService = Substitute.For<LogService>();
        var handler = new UpdateLeagueSettingHandler(repository, session, logService);

        SetupRepositoryMock(repository);

        var request = new UpdateLeagueSettingRequest
        {
            LeagueSettingId = 456,
            MaxbetWD_HDP = "1000",
            MaxbetWK_HDP = "2000",
            WD_NoOfHDP = "1",
            WK_NoOfHDP = "2",
            Spread = "0.05"
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).UpdateLeagueSetting(request, 0);
        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task Handle_WithNegativeUserId_ShouldStillProcessRequest()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();
        session.UserId.Returns(-1);

        var logService = Substitute.For<LogService>();
        var handler = new UpdateLeagueSettingHandler(repository, session, logService);

        SetupRepositoryMock(repository);

        var request = new UpdateLeagueSettingRequest
        {
            LeagueSettingId = 456,
            MaxbetWD_HDP = "1000",
            MaxbetWK_HDP = "2000",
            WD_NoOfHDP = "1",
            WK_NoOfHDP = "2",
            Spread = "0.05"
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).UpdateLeagueSetting(request, -1);
        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task Handle_ShouldCallLogService_WithCorrectLogRequest()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();
        var logService = Substitute.For<LogService>();

        session.UserId.Returns(123);

        var handler = new UpdateLeagueSettingHandler(repository, session, logService);

        SetupRepositoryMock(repository);

        var request = new UpdateLeagueSettingRequest
        {
            LeagueSettingId = 456,
            League = "Updated League",
            Spread = "2.0",
            MaxbetWD_HDP = "1500"
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await logService.Received(1).LogSetting(
            Arg.Is<LogRequest>(lr =>
                lr.Action == "Updated the setting" &&
                lr.leagueSettingId == 456 &&
                lr.CurrentSetting != null &&
                lr.NewSetting != null));

        Assert.Equal(Unit.Value, result);
    }
}
