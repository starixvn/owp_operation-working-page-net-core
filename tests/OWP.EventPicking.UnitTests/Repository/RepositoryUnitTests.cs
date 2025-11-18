using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanex.Data.Repository;
using Fanex.NAP.Client;
using MediatR;
using NSubstitute;
using Olympus.OWP.Log.Criteria;
using OWP.EventPicking;
using OWP.EventPicking.Api.Criteria;
using OWP.EventPicking.Api.Handlers;
using OWP.EventPicking.Api.Models;
using Xunit;

namespace OWP.EventPicking.UnitTests.Repository;

public class RepositoryUnitTests
{
    private readonly IDynamicRepository _dynamicRepository;
    private readonly INapSession _session;
    private readonly OWP.EventPicking.Repository _repository;

    public RepositoryUnitTests()
    {
        _dynamicRepository = Substitute.For<IDynamicRepository>();
        _session = Substitute.For<INapSession>();
        _repository = new RepositoryImp(_dynamicRepository, _session);
    }

    [Fact]
    public async Task QueryMainMatch_ShouldReturnEventPickingModels()
    {
        // Arrange
        var date = new DateTime(2024, 1, 15);
        var expectedModels = new List<EventPickingModel>
        {
            new EventPickingModel { InfoSiteMatchId = 1, MatchNo = "Match 1" },
            new EventPickingModel { InfoSiteMatchId = 2, MatchNo = "Match 2" }
        };

        _dynamicRepository.FetchAsync<EventPickingModel>(Arg.Any<QueryMainMatchCriteria>())
            .Returns(expectedModels);

        // Act
        var result = await _repository.QueryMainMatch(date);

        // Assert
        Assert.Equal(expectedModels.Count, result.Count);
        Assert.Equal(expectedModels[0].InfoSiteMatchId, result[0].InfoSiteMatchId);
        Assert.Equal(expectedModels[1].InfoSiteMatchId, result[1].InfoSiteMatchId);

        await _dynamicRepository.Received(1).FetchAsync<EventPickingModel>(
            Arg.Is<QueryMainMatchCriteria>(c => c.Date == date));
    }

    [Fact]
    public async Task QueryChildMatch_ShouldReturnEventPickingModels()
    {
        // Arrange
        var date = new DateTime(2024, 1, 15);
        var expectedModels = new List<EventPickingModel>
        {
            new EventPickingModel { InfoSiteMatchId = 3, MatchNo = "Child Match 1" }
        };

        _dynamicRepository.FetchAsync<EventPickingModel>(Arg.Any<QueryChildMatchCriteria>())
            .Returns(expectedModels);

        // Act
        var result = await _repository.QueryChildMatch(date);

        // Assert
        Assert.Equal(expectedModels.Count, result.Count);
        Assert.Equal(expectedModels[0].InfoSiteMatchId, result[0].InfoSiteMatchId);

        await _dynamicRepository.Received(1).FetchAsync<EventPickingModel>(
            Arg.Is<QueryChildMatchCriteria>(c => c.Date == date));
    }

    [Fact]
    public async Task QueryMovedEventDateMatch_ShouldReturnMatchesAndRAndDInfo()
    {
        // Arrange
        var date = new DateTime(2024, 1, 15);
        var expectedMatches = new List<EventPickingModel>
        {
            new EventPickingModel { InfoSiteMatchId = 1, MatchNo = "Moved Match 1" }
        };
        var expectedRAndDInfo = new List<RAndDInfo>
        {
            new RAndDInfo { InfositeId = 1, RAndDUpdate = "R&D Update 1" }
        };

        _dynamicRepository.FetchMultipleAsync<EventPickingModel, RAndDInfo>(Arg.Any<QueryMoveEventDateMatchCriteria>())
            .Returns((expectedMatches, expectedRAndDInfo));

        // Act
        var result = await _repository.QueryMovedEventDateMatch(date);

        // Assert
        Assert.Equal(expectedMatches.Count, result.Item1.Count);
        Assert.Equal(expectedRAndDInfo.Count, result.Item2.Count);
        Assert.Equal(expectedMatches[0].InfoSiteMatchId, result.Item1[0].InfoSiteMatchId);
        Assert.Equal(expectedRAndDInfo[0].InfositeId, result.Item2[0].InfositeId);

        await _dynamicRepository.Received(1).FetchMultipleAsync<EventPickingModel, RAndDInfo>(
            Arg.Is<QueryMoveEventDateMatchCriteria>(c => c.Date == date));
    }

    [Fact]
    public async Task GetLeagueSettings_ShouldReturnGroupedLeagueSettings()
    {
        // Arrange
        var leagueSettings = new List<LeagueSetting>
        {
            new LeagueSetting { OWPLeagueId = 1, LeagueName = "League 1" },
            new LeagueSetting { OWPLeagueId = 1, LeagueName = "League 1 Sub" },
            new LeagueSetting { OWPLeagueId = 2, LeagueName = "League 2" }
        };

        _dynamicRepository.FetchAsync<LeagueSetting>(Arg.Any<GetLeagueSettingCriteria>())
            .Returns(leagueSettings);

        // Act
        var result = await _repository.GetLeagueSettings();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(2, result[1].Count);
        Assert.Single(result[2]);
        Assert.Equal("League 1", result[1][0].LeagueName);
        Assert.Equal("League 2", result[2][0].LeagueName);
    }

    [Fact]
    public async Task GetTVList_ShouldReturnConditionsFromTVList()
    {
        // Arrange
        var tvList = new List<TVList>
        {
            new TVList { Label = "TV1", Value = "Value1" },
            new TVList { Label = "TV2", Value = "Value2" }
        };

        _dynamicRepository.FetchAsync<TVList>(Arg.Any<GetTVListCriteria>())
            .Returns(tvList);

        // Act
        var result = await _repository.GetTVList();

        // Assert
        var resultList = result.ToList();
        Assert.Equal(2, resultList.Count);
        Assert.Equal("TV1", resultList[0].Type);
        Assert.Equal("Value1", resultList[0].Value);
        Assert.Equal("TV2", resultList[1].Type);
        Assert.Equal("Value2", resultList[1].Value);
    }

    [Fact]
    public async Task InsertOrUpdateNoOfferMatch_ShouldExecuteCriteriaWithSerializedData()
    {
        // Arrange
        var request = new InsertOrUpdateNoOfferMatchRequest
        {
            NoOfferMatch = new List<NoOfferMatchInfo>
            {
                new NoOfferMatchInfo { InfoSiteMatchId = 123, IsNoOffer = true, ChildMatchId = null },
                new NoOfferMatchInfo { InfoSiteMatchId = 456, IsNoOffer = false, ChildMatchId = 789 }
            }
        };
        _session.UserId.Returns(456);

        // Act
        await _repository.InsertOrUpdateNoOfferMatch(request);

        // Assert
        // Verify that ExecuteAsync was called twice - once for the criteria and once for logging
        await _dynamicRepository.Received(1).ExecuteAsync(
            Arg.Is<InsertOrUpdateNoOfferMatchCriteria>(c =>
                c.MatchInfoJson.Contains("123") && // Only the match with ChildMatchId == null should be included
                !c.MatchInfoJson.Contains("456") && // The match with ChildMatchId != null should be excluded
                c.ActorId == 456));

        await _dynamicRepository.Received(1).ExecuteAsync(
            Arg.Is<OwpLogCriteria>(c =>
                c.Action == "EventPicking-UpdateNoOfferMatch" &&
                c.Payload.Contains("123") &&
                !c.Payload.Contains("456")));
    }

    [Fact]
    public async Task GetProviderArrangement_ShouldReturnOrderedProviderValues()
    {
        // Arrange
        var providerInfo = new List<ProviderInfo>
        {
            new ProviderInfo { Order = 2, Value = "Provider2" },
            new ProviderInfo { Order = 1, Value = "Provider1" },
            new ProviderInfo { Order = 3, Value = "Provider3" }
        };

        _dynamicRepository.FetchAsync<ProviderInfo>(Arg.Any<GetProviderCriteria>())
            .Returns(providerInfo);

        // Act
        var result = await _repository.GetProviderArrangement();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal("Provider1", result[0]);
        Assert.Equal("Provider2", result[1]);
        Assert.Equal("Provider3", result[2]);
    }

    [Fact]
    public async Task GetRAndDUpdateInfo_ShouldReturnRAndDInfoList()
    {
        // Arrange
        var date = new DateTime(2024, 1, 15);
        var expectedInfo = new List<RAndDInfo>
        {
            new RAndDInfo { InfositeId = 1, RAndDUpdate = "R&D Update 1" }
        };

        _dynamicRepository.FetchAsync<RAndDInfo>(Arg.Any<QueryRAndDInfoCriteria>())
            .Returns(expectedInfo);

        // Act
        var result = await _repository.GetRAndDUpdateInfo(date);

        // Assert
        Assert.Equal(expectedInfo.Count, result.Count);
        Assert.Equal(expectedInfo[0].InfositeId, result[0].InfositeId);

        await _dynamicRepository.Received(1).FetchAsync<RAndDInfo>(
            Arg.Is<QueryRAndDInfoCriteria>(c => c.Date == date));
    }

    [Fact]
    public async Task GetUpdateType_ShouldReturnDistinctOptions()
    {
        // Arrange
        var options = new List<Option>
        {
            new Option { Value = "Type1" },
            new Option { Value = "Type1" }, // Duplicate
            new Option { Value = "Type2" }
        };

        _dynamicRepository.FetchAsync<Option>(Arg.Any<QueryUpdateTypeCriteria>())
            .Returns(options);

        // Act
        var result = await _repository.GetUpdateType();

        // Assert
        Assert.Equal(2, result.Count); // Should be distinct
        Assert.Contains(result, o => o.Value == "Type1");
        Assert.Contains(result, o => o.Value == "Type2");
    }

    [Fact]
    public async Task GetLog_ShouldReturnLogModels()
    {
        // Arrange
        var infoSiteMatchId = 123;
        var expectedLogs = new List<LogModel>
        {
            new LogModel { Date = DateTime.Now, Action = "Log 1" },
            new LogModel { Date = DateTime.Now, Action = "Log 2" }
        };

        _dynamicRepository.FetchAsync<LogModel>(Arg.Any<GetLogCriteria>())
            .Returns(expectedLogs);

        // Act
        var result = await _repository.GetLog(infoSiteMatchId);

        // Assert
        Assert.Equal(expectedLogs.Count, result.Count);
        Assert.Equal(expectedLogs[0].Action, result[0].Action);
        Assert.Equal(expectedLogs[1].Action, result[1].Action);

        await _dynamicRepository.Received(1).FetchAsync<LogModel>(
            Arg.Is<GetLogCriteria>(c => c.InfoSiteMatchId == infoSiteMatchId));
    }

    [Fact]
    public async Task UpdateConfirmed_ShouldExecuteCriteria()
    {
        // Arrange
        var criteria = new UpdateConfirmedMatchCriteria
        {
            InfoSiteMatchIds = "123,456",
            Confirmed = true,
            ModifiedBy = "testuser"
        };

        // Act
        await _repository.UpdateConfirmed(criteria);

        // Assert
        await _dynamicRepository.Received(1).ExecuteAsync(criteria);
    }

    [Fact]
    public async Task QueryLeagueSetting_ShouldReturnLeagueSettingInfoSenders()
    {
        // Arrange
        var expectedSettings = new List<LeagueSettingInfoSender>
        {
            new LeagueSettingInfoSender { LeagueSettingId = 1, League = "Setting 1" }
        };

        _dynamicRepository.FetchAsync<LeagueSettingInfoSender>(Arg.Any<GetNewLeagueSettingCriteria>())
            .Returns(expectedSettings);

        // Act
        var result = await _repository.QueryLeagueSetting();

        // Assert
        var resultList = result.ToList();
        Assert.Equal(expectedSettings.Count, resultList.Count);
        Assert.Equal(expectedSettings[0].LeagueSettingId, resultList[0].LeagueSettingId);
    }

    [Fact]
    public async Task UpdateEvent_ShouldExecuteCriteria()
    {
        // Arrange
        var criteria = new UpdateMaxbetSpreadCriteria
        {
            TargetJson = "{\"matchId\":123,\"maxbetSpread\":1.5}",
            ActorId = 456
        };

        // Act
        await _repository.UpdateEvent(criteria);

        // Assert
        await _dynamicRepository.Received(1).ExecuteAsync(criteria);
    }

    [Fact]
    public async Task ExecuteNonQuery_ShouldExecuteCriteria()
    {
        // Arrange
        var criteria = Substitute.For<INonQueryCommand>();

        // Act
        await _repository.ExecuteNonQuery(criteria);

        // Assert
        await _dynamicRepository.Received(1).ExecuteAsync(criteria);
    }

    [Fact]
    public async Task QueryEventPickingChildMatch_ShouldReturnEventPickingModels()
    {
        // Arrange
        var date = new DateTime(2024, 1, 15);
        var expectedModels = new List<EventPickingModel>
        {
            new EventPickingModel { InfoSiteMatchId = 4, MatchNo = "Event Picking Child 1" }
        };

        _dynamicRepository.FetchAsync<EventPickingModel>(Arg.Any<QueryEventPickingChildMatchCriteria>())
            .Returns(expectedModels);

        // Act
        var result = await _repository.QueryEventPickingChildMatch(date);

        // Assert
        Assert.Equal(expectedModels.Count, result.Count);
        Assert.Equal(expectedModels[0].InfoSiteMatchId, result[0].InfoSiteMatchId);

        await _dynamicRepository.Received(1).FetchAsync<EventPickingModel>(
            Arg.Is<QueryEventPickingChildMatchCriteria>(c => c.Date == date));
    }
}
