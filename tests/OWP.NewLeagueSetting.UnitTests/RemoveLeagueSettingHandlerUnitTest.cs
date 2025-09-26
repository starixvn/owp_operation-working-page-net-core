using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fanex.NAP.Client;
using MediatR;
using NSubstitute;
using OWP.CQRS;
using OWP.NewLeagueSetting.Command;
using OWP.NewLeagueSetting.Criteria;
using OWP.NewLeagueSetting.Data;
using OWP.NewLeagueSetting.Models;
using OWP.NewLeagueSetting.Service;
using Xunit;

namespace OWP.NewLeagueSetting.UnitTests;

public class RemoveLeagueSettingHandlerUnitTest
{
    [Fact]
    public async Task Handle_ShouldCreateRemoveSettingCriteria_AndCallRepository_AndReturnUnit()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();

        session.UserId.Returns(123);

        var changeService = Substitute.For<ILeagueSettingChangeService>();
        var handler = new RemoveLeagueSettingHandler(repository, session, changeService);

        var emptyLeagueSetting = new LeagueSettingInfoSender();
        repository.QueryLeagueSettingDetail(Arg.Any<int>()).Returns(emptyLeagueSetting);
        changeService.LogProcessingChanges(Arg.Any<LeagueSettingChangeModel>(), Arg.Any<LeagueSettingChangeModel>())
            .Returns(new List<LogDetail>());

        var request = new RemoveLeagueSettingRequest(456);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).ExecuteNonQueryCommand(
            Arg.Is<RemoveSettingCriteria>(c =>
                c.LeagueSettingId == 456 &&
                c.ActorId == 123));
        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task Handle_WithDifferentLeagueSettingId_ShouldCreateCorrectCriteria()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();

        session.UserId.Returns(789);

        var changeService = Substitute.For<ILeagueSettingChangeService>();
        var handler = new RemoveLeagueSettingHandler(repository, session, changeService);

        var emptyLeagueSetting = new LeagueSettingInfoSender();
        repository.QueryLeagueSettingDetail(Arg.Any<int>()).Returns(emptyLeagueSetting);
        changeService.LogProcessingChanges(Arg.Any<LeagueSettingChangeModel>(), Arg.Any<LeagueSettingChangeModel>())
            .Returns(new List<LogDetail>());

        var request = new RemoveLeagueSettingRequest(999);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).ExecuteNonQueryCommand(
            Arg.Is<RemoveSettingCriteria>(c =>
                c.LeagueSettingId == 999 &&
                c.ActorId == 789));
        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task Handle_WithZeroUserId_ShouldStillProcessRequest()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();

        session.UserId.Returns(0);

        var changeService = Substitute.For<ILeagueSettingChangeService>();
        var handler = new RemoveLeagueSettingHandler(repository, session, changeService);

        var emptyLeagueSetting = new LeagueSettingInfoSender();
        repository.QueryLeagueSettingDetail(Arg.Any<int>()).Returns(emptyLeagueSetting);
        changeService.LogProcessingChanges(Arg.Any<LeagueSettingChangeModel>(), Arg.Any<LeagueSettingChangeModel>())
            .Returns(new List<LogDetail>());

        var request = new RemoveLeagueSettingRequest(456);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).ExecuteNonQueryCommand(
            Arg.Is<RemoveSettingCriteria>(c =>
                c.LeagueSettingId == 456 &&
                c.ActorId == 0));
        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task Handle_WithNegativeUserId_ShouldStillProcessRequest()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();

        session.UserId.Returns(-1);

        var changeService = Substitute.For<ILeagueSettingChangeService>();
        var handler = new RemoveLeagueSettingHandler(repository, session, changeService);

        var emptyLeagueSetting = new LeagueSettingInfoSender();
        repository.QueryLeagueSettingDetail(Arg.Any<int>()).Returns(emptyLeagueSetting);
        changeService.LogProcessingChanges(Arg.Any<LeagueSettingChangeModel>(), Arg.Any<LeagueSettingChangeModel>())
            .Returns(new List<LogDetail>());

        var request = new RemoveLeagueSettingRequest(456);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).ExecuteNonQueryCommand(
            Arg.Is<RemoveSettingCriteria>(c =>
                c.LeagueSettingId == 456 &&
                c.ActorId == -1));
        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task Handle_WithZeroLeagueSettingId_ShouldStillProcessRequest()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();

        session.UserId.Returns(123);

        var changeService = Substitute.For<ILeagueSettingChangeService>();
        var handler = new RemoveLeagueSettingHandler(repository, session, changeService);

        var emptyLeagueSetting = new LeagueSettingInfoSender();
        repository.QueryLeagueSettingDetail(Arg.Any<int>()).Returns(emptyLeagueSetting);
        changeService.LogProcessingChanges(Arg.Any<LeagueSettingChangeModel>(), Arg.Any<LeagueSettingChangeModel>())
            .Returns(new List<LogDetail>());

        var request = new RemoveLeagueSettingRequest(0);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).ExecuteNonQueryCommand(
            Arg.Is<RemoveSettingCriteria>(c =>
                c.LeagueSettingId == 0 &&
                c.ActorId == 123));
        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task Handle_WithNegativeLeagueSettingId_ShouldStillProcessRequest()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();

        session.UserId.Returns(123);

        var changeService = Substitute.For<ILeagueSettingChangeService>();
        var handler = new RemoveLeagueSettingHandler(repository, session, changeService);

        var emptyLeagueSetting = new LeagueSettingInfoSender();
        repository.QueryLeagueSettingDetail(Arg.Any<int>()).Returns(emptyLeagueSetting);
        changeService.LogProcessingChanges(Arg.Any<LeagueSettingChangeModel>(), Arg.Any<LeagueSettingChangeModel>())
            .Returns(new List<LogDetail>());

        var request = new RemoveLeagueSettingRequest(-1);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).ExecuteNonQueryCommand(
            Arg.Is<RemoveSettingCriteria>(c =>
                c.LeagueSettingId == -1 &&
                c.ActorId == 123));
        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task Handle_ShouldQueryLeagueSettingDetail_AndCallChangeService_AndSerializeLogData()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();
        var changeService = Substitute.For<ILeagueSettingChangeService>();

        session.UserId.Returns(123);

        var leagueSetting = new LeagueSettingInfoSender
        {
            League = "Test League",
            LeagueGroup = "Test Group",
            Spread_HDP_Value = 0.01m,
            Spread_OU_Value = 0.01m,
            MaxbetWD_HDP_Value = 1000000m,
            MaxbetWD_HDP_Note = "1000K",
            WD_NoOfHDP_Value = "5",
            MaxbetWK_HDP_Value = 2000000m,
            MaxbetWK_HDP_Note = "2000K",
            WK_NoOfHDP_Value = "10",
            LstNote = "Test Note",
            Remark2 = "Test Remark 2",
            Comment = "Test Comment",
            FullConditionDescription = System.Text.Json.JsonSerializer.Serialize(new LeagueSettingCondition
            {
                HasTv = false,
                HasIpTv = false,
                IpTv = new List<Condition>(),
                Round = new List<Condition>(),
                Division = new List<Condition>(),
                Remark = "Test Remark"
            }),
            IsBlackList = false,
            NoOfferLive = false
        };

        var logData = new List<LogDetail>
        {
            new LogDetail { Field = "League name", OldValue = "Test League", NewValue = "" },
            new LogDetail { Field = "FT Spread", OldValue = "1.0", NewValue = "" }
        };

        repository.QueryLeagueSettingDetail(456).Returns(leagueSetting);
        changeService.LogProcessingChanges(Arg.Any<LeagueSettingChangeModel>(), Arg.Any<LeagueSettingChangeModel>())
            .Returns(logData);

        var handler = new RemoveLeagueSettingHandler(repository, session, changeService);

        var request = new RemoveLeagueSettingRequest(456);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).QueryLeagueSettingDetail(456);
        
        changeService.Received(1).LogProcessingChanges(
            Arg.Is<LeagueSettingChangeModel>(m => m.League == "Test League"),
            Arg.Is<LeagueSettingChangeModel>(m => m.League == null));

        await repository.Received(1).ExecuteNonQueryCommand(
            Arg.Is<RemoveSettingCriteria>(c =>
                c.LeagueSettingId == 456 &&
                c.ActorId == 123 &&
                !string.IsNullOrEmpty(c.LogData)));

        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task Handle_ShouldMapLeagueSettingToChangeModel_ForLogging()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();
        var changeService = Substitute.For<ILeagueSettingChangeService>();

        session.UserId.Returns(123);

        var leagueSetting = new LeagueSettingInfoSender
        {
            League = "Test League",
            LeagueGroup = "Test Group",
            Spread_HDP_Value = 0.01m,
            Spread_OU_Value = 0.01m,
            MaxbetWD_HDP_Value = 1000000m,
            MaxbetWD_HDP_Note = "1000K",
            WD_NoOfHDP_Value = "5",
            MaxbetWK_HDP_Value = 2000000m,
            MaxbetWK_HDP_Note = "2000K",
            WK_NoOfHDP_Value = "10",
            LstNote = "Test Note",
            Remark2 = "Test Remark 2",
            Comment = "Test Comment",
            FullConditionDescription = System.Text.Json.JsonSerializer.Serialize(new LeagueSettingCondition
            {
                HasTv = true,
                HasIpTv = false,
                IpTv = new List<Condition> { new Condition("IpTv", "Test") },
                Round = new List<Condition> { new Condition("Round", "1") },
                Division = new List<Condition> { new Condition("Division", "A") },
                Remark = "Test Remark"
            }),
            IsBlackList = true,
            NoOfferLive = true
        };

        repository.QueryLeagueSettingDetail(456).Returns(leagueSetting);
        changeService.LogProcessingChanges(Arg.Any<LeagueSettingChangeModel>(), Arg.Any<LeagueSettingChangeModel>())
            .Returns(new List<LogDetail>());

        var handler = new RemoveLeagueSettingHandler(repository, session, changeService);

        var request = new RemoveLeagueSettingRequest(456);

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert
        changeService.Received(1).LogProcessingChanges(
            Arg.Any<LeagueSettingChangeModel>(),
            Arg.Any<LeagueSettingChangeModel>());
    }

    [Fact]
    public async Task Handle_WithEmptyLeagueSetting_ShouldStillLogChanges()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();
        var changeService = Substitute.For<ILeagueSettingChangeService>();

        session.UserId.Returns(123);

        var emptyLeagueSetting = new LeagueSettingInfoSender();

        repository.QueryLeagueSettingDetail(456).Returns(emptyLeagueSetting);
        changeService.LogProcessingChanges(Arg.Any<LeagueSettingChangeModel>(), Arg.Any<LeagueSettingChangeModel>())
            .Returns(new List<LogDetail>());

        var handler = new RemoveLeagueSettingHandler(repository, session, changeService);

        var request = new RemoveLeagueSettingRequest(456);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).QueryLeagueSettingDetail(456);
        changeService.Received(1).LogProcessingChanges(
            Arg.Any<LeagueSettingChangeModel>(),
            Arg.Any<LeagueSettingChangeModel>());

        Assert.Equal(Unit.Value, result);
    }
}
