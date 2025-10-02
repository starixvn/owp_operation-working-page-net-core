using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fanex.NAP.Client;
using NSubstitute;
using OWP.NewLeagueSetting.Command;
using OWP.NewLeagueSetting.Criteria;
using OWP.NewLeagueSetting.Data;
using OWP.NewLeagueSetting.Models;
using OWP.NewLeagueSetting.Service;
using Xunit;

namespace OWP.NewLeagueSetting.UnitTests.Service;

public class LoggerServiceImpUnitTests
{
    [Fact]
    public async Task LogSetting_WithValidRequest_ShouldCallChangeServiceAndRepository()
    {
        // Arrange
        var changeService = Substitute.For<ILeagueSettingChangeService>();
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();

        var logService = new LoggerServiceImp(changeService, repository, session);

        var currentSetting = new LeagueSettingChangeModel { League = "Test League" };
        var newSetting = new LeagueSettingChangeModel { League = "Updated League" };
        var request = new LogRequest("Updated the setting", 123, currentSetting, newSetting);

        var logDetails = new List<LogDetail>
        {
            new LogDetail { Field = "League Name", OldValue = "Test League", NewValue = "Updated League" }
        };

        changeService.LogProcessingChanges(currentSetting, newSetting).Returns(logDetails);
        session.UserName.Returns("testuser");

        // Act
        await logService.LogSetting(request);

        // Assert
        changeService.Received(1).LogProcessingChanges(currentSetting, newSetting);
        await repository.Received(1).ExecuteNonQueryCommand(Arg.Is<InsertLogCriteria>(criteria =>
            criteria.Action == "Updated the setting" &&
            criteria.SettingId == 123 &&
            criteria.ModifiedBy == "testuser" &&
            criteria.Content.Contains("League Name") &&
            criteria.Content.Contains("Test League") &&
            criteria.Content.Contains("Updated League")
        ));
    }

    [Fact]
    public async Task LogSetting_WithEmptyChanges_ShouldStillLogToRepository()
    {
        // Arrange
        var changeService = Substitute.For<ILeagueSettingChangeService>();
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();

        var logService = new LoggerServiceImp(changeService, repository, session);

        var currentSetting = new LeagueSettingChangeModel { League = "Test League" };
        var newSetting = new LeagueSettingChangeModel { League = "Test League" };
        var request = new LogRequest("No changes", 456, currentSetting, newSetting);

        changeService.LogProcessingChanges(currentSetting, newSetting).Returns(new List<LogDetail>());
        session.UserName.Returns("testuser");

        // Act
        await logService.LogSetting(request);

        // Assert
        changeService.Received(1).LogProcessingChanges(currentSetting, newSetting);
        await repository.Received(1).ExecuteNonQueryCommand(Arg.Is<InsertLogCriteria>(criteria =>
            criteria.Action == "No changes" &&
            criteria.SettingId == 456 &&
            criteria.ModifiedBy == "testuser" &&
            criteria.Content == "[]"
        ));
    }

    [Fact]
    public async Task LogSetting_WithNullChanges_ShouldHandleGracefully()
    {
        // Arrange
        var changeService = Substitute.For<ILeagueSettingChangeService>();
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();

        var logService = new LoggerServiceImp(changeService, repository, session);

        var currentSetting = new LeagueSettingChangeModel { League = "Test League" };
        var newSetting = new LeagueSettingChangeModel { League = "Updated League" };
        var request = new LogRequest("Updated the setting", 789, currentSetting, newSetting);

        changeService.LogProcessingChanges(currentSetting, newSetting).Returns((List<LogDetail>)null);
        session.UserName.Returns("testuser");

        // Act
        await logService.LogSetting(request);

        // Assert
        changeService.Received(1).LogProcessingChanges(currentSetting, newSetting);
        await repository.Received(1).ExecuteNonQueryCommand(Arg.Is<InsertLogCriteria>(criteria =>
            criteria.Action == "Updated the setting" &&
            criteria.SettingId == 789 &&
            criteria.ModifiedBy == "testuser" &&
            criteria.Content == "null"
        ));
    }

    [Fact]
    public void MapToChangeModel_WithLeagueSettingInfoSender_ShouldMapCorrectly()
    {
        // Arrange
        var leagueSetting = new LeagueSettingInfoSender
        {
            League = "Premier League",
            LeagueGroup = "Football",
            LstNote = "Test Note",
            Remark2 = "Test Remark 2",
            IsBlackList = false,
            NoOfferLive = true,
            FullConditionDescription = System.Text.Json.JsonSerializer.Serialize(new LeagueSettingCondition
            {
                HasTv = true,
                HasIpTv = false,
                IpTv = new List<Condition> { new Condition("Equals", "IPTV1") },
                Round = new List<Condition> { new Condition("Equals", "Round1") },
                Division = new List<Condition> { new Condition("Equals", "Div1") },
                Remark = "Condition Remark"
            })
        };

        // Act
        var result = LoggerServiceImp.MapToChangeModel(leagueSetting);

        // Assert
        Assert.Equal("Premier League", result.League);
        Assert.Equal("Football", result.LeagueGroup);
        Assert.Equal("Test Note", result.LstNote);
        Assert.Equal("Test Remark 2", result.Remark2);
        Assert.True(result.HasTv);
        Assert.False(result.HasIpTv);
        Assert.Single(result.IpTv);
        Assert.Equal("IPTV1", result.IpTv[0].Value);
        Assert.Single(result.Round);
        Assert.Equal("Round1", result.Round[0].Value);
        Assert.Single(result.Division);
        Assert.Equal("Div1", result.Division[0].Value);
        Assert.Equal("Condition Remark", result.Remark);
        Assert.False(result.IsBlackList);
        Assert.True(result.NoOfferLive);
    }

    [Fact]
    public void MapToChangeModel_WithNullConditionProperties_ShouldHandleGracefully()
    {
        // Arrange
        var leagueSetting = new LeagueSettingInfoSender
        {
            League = "Test League",
            LeagueGroup = "Test Group",
            FullConditionDescription = System.Text.Json.JsonSerializer.Serialize(new LeagueSettingCondition
            {
                HasTv = false,
                HasIpTv = true,
                IpTv = null,
                Round = null,
                Division = null,
                Remark = null
            })
        };

        // Act
        var result = LoggerServiceImp.MapToChangeModel(leagueSetting);

        // Assert
        Assert.Equal("Test League", result.League);
        Assert.Equal("Test Group", result.LeagueGroup);
        Assert.False(result.HasTv);
        Assert.True(result.HasIpTv);
        Assert.NotNull(result.IpTv);
        Assert.Empty(result.IpTv);
        Assert.NotNull(result.Round);
        Assert.Empty(result.Round);
        Assert.NotNull(result.Division);
        Assert.Empty(result.Division);
        Assert.Null(result.Remark);
    }

    [Fact]
    public void MapToChangeModel_WithUpdateLeagueSettingRequest_ShouldMapCorrectly()
    {
        // Arrange
        var request = new UpdateLeagueSettingRequest
        {
            League = "La Liga",
            LeagueGroup = "Football",
            Spread = "2.0",
            MaxbetWD_HDP = "1500",
            WD_NoOfHDP = "7",
            MaxbetWK_HDP = "2500",
            WK_NoOfHDP = "12",
            LstNote = "Request Note",
            Remark2 = "Request Remark 2",
            HasTv = true,
            HasIpTv = false,
            IpTv = new List<Condition> { new Condition("Equals", "IPTV2") },
            Round = new List<Condition> { new Condition("Equals", "Round2") },
            Division = new List<Condition> { new Condition("Equals", "Div2") },
            Remark = "Request Remark",
            IsBlackList = true,
            NoOfferLive = false
        };

        // Act
        var result = LoggerServiceImp.MapToChangeModel(request);

        // Assert
        Assert.Equal("La Liga", result.League);
        Assert.Equal("Football", result.LeagueGroup);
        Assert.Equal("2.0", result.Spread);
        Assert.Equal("1500", result.MaxbetWD_HDP);
        Assert.Equal("7", result.WD_NoOfHDP);
        Assert.Equal("2500", result.MaxbetWK_HDP);
        Assert.Equal("12", result.WK_NoOfHDP);
        Assert.Equal("Request Note", result.LstNote);
        Assert.Equal("Request Remark 2", result.Remark2);
        Assert.True(result.HasTv);
        Assert.False(result.HasIpTv);
        Assert.Single(result.IpTv);
        Assert.Equal("IPTV2", result.IpTv[0].Value);
        Assert.Single(result.Round);
        Assert.Equal("Round2", result.Round[0].Value);
        Assert.Single(result.Division);
        Assert.Equal("Div2", result.Division[0].Value);
        Assert.Equal("Request Remark", result.Remark);
        Assert.True(result.IsBlackList);
        Assert.False(result.NoOfferLive);
    }

    [Fact]
    public void MapToChangeModel_WithNullRequestProperties_ShouldHandleGracefully()
    {
        // Arrange
        var request = new UpdateLeagueSettingRequest
        {
            League = "Test League",
            LeagueGroup = "Test Group",
            IpTv = null,
            Round = null,
            Division = null,
            Remark = null
        };

        // Act
        var result = LoggerServiceImp.MapToChangeModel(request);

        // Assert
        Assert.Equal("Test League", result.League);
        Assert.Equal("Test Group", result.LeagueGroup);
        Assert.NotNull(result.IpTv);
        Assert.Empty(result.IpTv);
        Assert.NotNull(result.Round);
        Assert.Empty(result.Round);
        Assert.NotNull(result.Division);
        Assert.Empty(result.Division);
        Assert.Null(result.Remark);
    }

    [Fact]
    public async Task LogSetting_WithDifferentActions_ShouldLogCorrectAction()
    {
        // Arrange
        var changeService = Substitute.For<ILeagueSettingChangeService>();
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();

        var logService = new LoggerServiceImp(changeService, repository, session);

        var currentSetting = new LeagueSettingChangeModel { League = "Test League" };
        var newSetting = new LeagueSettingChangeModel { League = "Updated League" };

        changeService.LogProcessingChanges(Arg.Any<LeagueSettingChangeModel>(), Arg.Any<LeagueSettingChangeModel>())
            .Returns(new List<LogDetail>());
        session.UserName.Returns("testuser");

        // Act
        await logService.LogSetting(new LogRequest("Created the setting", 111, currentSetting, newSetting));
        await logService.LogSetting(new LogRequest("Updated the setting", 222, currentSetting, newSetting));
        await logService.LogSetting(new LogRequest("Deleted the setting", 333, currentSetting, newSetting));

        // Assert
        await repository.Received(1).ExecuteNonQueryCommand(Arg.Is<InsertLogCriteria>(c => c.Action == "Created the setting" && c.SettingId == 111));
        await repository.Received(1).ExecuteNonQueryCommand(Arg.Is<InsertLogCriteria>(c => c.Action == "Updated the setting" && c.SettingId == 222));
        await repository.Received(1).ExecuteNonQueryCommand(Arg.Is<InsertLogCriteria>(c => c.Action == "Deleted the setting" && c.SettingId == 333));
    }

    [Fact]
    public async Task LogSetting_ShouldSetCorrectCreatedDate()
    {
        // Arrange
        var changeService = Substitute.For<ILeagueSettingChangeService>();
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();

        var logService = new LoggerServiceImp(changeService, repository, session);

        var currentSetting = new LeagueSettingChangeModel { League = "Test League" };
        var newSetting = new LeagueSettingChangeModel { League = "Updated League" };
        var request = new LogRequest("Updated the setting", 123, currentSetting, newSetting);

        changeService.LogProcessingChanges(currentSetting, newSetting).Returns(new List<LogDetail>());
        session.UserName.Returns("testuser");

        // Act
        await logService.LogSetting(request);

        // Assert
        await repository.Received(1).ExecuteNonQueryCommand(Arg.Is<InsertLogCriteria>(criteria =>
            criteria.CreatedDate != default(DateTime)
        ));
    }
}
