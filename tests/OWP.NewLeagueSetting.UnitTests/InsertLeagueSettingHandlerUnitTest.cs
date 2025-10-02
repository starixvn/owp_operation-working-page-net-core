using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
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

public class InsertLeagueSettingHandlerUnitTest
{
    [Fact]
    public async Task Handle_ShouldBuildStandardizedInfo_AndCallRepository_AndReturnLeagueSettingId()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();
        var mapper = Substitute.For<IMapper>();
        var logService = Substitute.For<LogService>();

        session.UserId.Returns(123);

        var handler = new InsertLeagueSettingHandler(repository, session, mapper, logService);

        var request = new InsertLeagueSettingRequest
        {
            MaxbetWD_HDP = "1000",
            MaxbetWK_HDP = "2000",
            WD_NoOfHDP = "1",
            WK_NoOfHDP = "2",
            Spread = "0.05"
        };

        var criteria = new InsertLeagueSettingCriteria
        {
            LeagueSettingId = 456
        };

        var leagueSettingDetail = new OWP.NewLeagueSetting.Models.LeagueSettingInfoSender
        {
            League = "Test League",
            Spread_HDP_Value = 0.01m,
            Spread_OU_Value = 0.01m,
            MaxbetWD_HDP_Value = 1000000m,
            MaxbetWD_HDP_Note = "1000K"
        };

        mapper.Map<InsertLeagueSettingCriteria>(request).Returns(criteria);
        repository.QueryLeagueSettingDetail(Arg.Any<int>()).Returns(leagueSettingDetail);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).ExecuteNonQueryCommand(
            Arg.Is<InsertLeagueSettingCriteria>(c =>
                c.ActorId == 123 &&
                c.LeagueSettingId == 456));

        Assert.Equal(456, result);
    }

    [Fact]
    public async Task Handle_WithDifferentValues_ShouldCallRepositoryWithCorrectActorId()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();
        var mapper = Substitute.For<IMapper>();
        var logService = Substitute.For<LogService>();

        session.UserId.Returns(789);

        var handler = new InsertLeagueSettingHandler(repository, session, mapper, logService);

        var request = new InsertLeagueSettingRequest
        {
            MaxbetWD_HDP = "3000",
            MaxbetWK_HDP = "4000",
            WD_NoOfHDP = "3",
            WK_NoOfHDP = "4",
            Spread = "0.10"
        };

        var criteria = new InsertLeagueSettingCriteria
        {
            LeagueSettingId = 999
        };

        var leagueSettingDetail = new OWP.NewLeagueSetting.Models.LeagueSettingInfoSender
        {
            League = "Test League 2",
            Spread_HDP_Value = 0.10m,
            Spread_OU_Value = 0.10m,
            MaxbetWD_HDP_Value = 3000000m,
            MaxbetWD_HDP_Note = "3000K"
        };

        mapper.Map<InsertLeagueSettingCriteria>(request).Returns(criteria);
        repository.QueryLeagueSettingDetail(Arg.Any<int>()).Returns(leagueSettingDetail);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).ExecuteNonQueryCommand(
            Arg.Is<InsertLeagueSettingCriteria>(c =>
                c.ActorId == 789 &&
                c.LeagueSettingId == 999));

        Assert.Equal(999, result);
    }

    [Fact]
    public async Task Handle_ShouldCallLogService_WithCorrectLogRequest()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();
        var mapper = Substitute.For<IMapper>();
        var logService = Substitute.For<LogService>();

        session.UserId.Returns(123);

        var handler = new InsertLeagueSettingHandler(repository, session, mapper, logService);

        var request = new InsertLeagueSettingRequest
        {
            League = "Test League",
            Spread = "1.0",
            MaxbetWD_HDP = "1000",
            LeagueSettingId = 456
        };

        var criteria = new InsertLeagueSettingCriteria
        {
            LeagueSettingId = 456
        };

        var leagueSettingDetail = new OWP.NewLeagueSetting.Models.LeagueSettingInfoSender
        {
            League = "Test League",
            Spread_HDP_Value = 0.01m,
            Spread_OU_Value = 0.01m,
            MaxbetWD_HDP_Value = 1000000m,
            MaxbetWD_HDP_Note = "1000K"
        };

        mapper.Map<InsertLeagueSettingCriteria>(request).Returns(criteria);
        repository.QueryLeagueSettingDetail(Arg.Any<int>()).Returns(leagueSettingDetail);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).QueryLeagueSettingDetail(456);
        await logService.Received(1).LogSetting(
            Arg.Is<LogRequest>(lr =>
                lr.Action == "Created the setting" &&
                lr.leagueSettingId == 456 &&
                lr.CurrentSetting != null &&
                lr.NewSetting != null));

        Assert.Equal(456, result);
    }

    [Fact]
    public async Task Handle_ShouldMapRequestToChangeModel_ForLogging()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();
        var mapper = Substitute.For<IMapper>();
        var logService = Substitute.For<LogService>();

        session.UserId.Returns(123);

        var handler = new InsertLeagueSettingHandler(repository, session, mapper, logService);

        var request = new InsertLeagueSettingRequest
        {
            League = "Test League",
            LeagueGroup = "Test Group",
            Spread = "1.0",
            MaxbetWD_HDP = "1000",
            WD_NoOfHDP = "5",
            MaxbetWK_HDP = "2000",
            WK_NoOfHDP = "10",
            LstNote = "Test Note",
            Remark2 = "Test Remark 2",
            Comment = "Test Comment",
            HasTv = false,
            HasIpTv = false,
            IpTv = new List<Condition>(),
            Round = new List<Condition>(),
            Division = new List<Condition>(),
            Remark = "Test Remark",
            IsBlackList = false,
            NoOfferLive = false,
            LeagueSettingId = 456
        };

        var criteria = new InsertLeagueSettingCriteria
        {
            LeagueSettingId = 456
        };

        var leagueSettingDetail = new OWP.NewLeagueSetting.Models.LeagueSettingInfoSender
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
            FullConditionDescription = "{\"HasTv\":false,\"HasIpTv\":false,\"IpTv\":[],\"Round\":[],\"Division\":[],\"Remark\":\"Test Remark\"}",
            IsBlackList = false,
            NoOfferLive = false
        };

        mapper.Map<InsertLeagueSettingCriteria>(request).Returns(criteria);
        repository.QueryLeagueSettingDetail(Arg.Any<int>()).Returns(leagueSettingDetail);

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).QueryLeagueSettingDetail(456);
        await logService.Received(1).LogSetting(
            Arg.Is<LogRequest>(lr =>
                lr.Action == "Created the setting" &&
                lr.leagueSettingId == 456 &&
                lr.CurrentSetting != null &&
                lr.NewSetting != null));
    }
}