using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fanex.NAP.Client;
using NSubstitute;
using Newtonsoft.Json;
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
            NoOfferLive = 1,
            FullConditionDescription = JsonConvert.SerializeObject(new LeagueSettingCondition
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
        Assert.Equal(1, result.NoOfferLive);
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
            NoOfferLive = 0
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
        Assert.Equal(0, result.NoOfferLive);
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

    [Fact]
    public void MapToChangeModel_WithInsertLeagueSettingRequest_ShouldMapCorrectly()
    {
        // Arrange
        var request = new InsertLeagueSettingRequest
        {
            League = "Bundesliga",
            LeagueGroup = "Football",
            Spread = "3.0",
            MaxbetWD_HDP = "2000",
            WD_NoOfHDP = "8",
            MaxbetWK_HDP = "3000",
            WK_NoOfHDP = "15",
            LstNote = "Insert Note",
            Remark2 = "Insert Remark 2",
            HasTv = false,
            HasIpTv = true,
            IpTv = new List<Condition> { new Condition("Contains", "IPTV3") },
            Round = new List<Condition> { new Condition("Contains", "Round3") },
            Division = new List<Condition> { new Condition("Contains", "Div3") },
            Remark = "Insert Remark",
            IsBlackList = false,
            NoOfferLive = 1
        };

        // Act
        var result = LoggerServiceImp.MapToChangeModel(request);

        // Assert
        Assert.Equal("Bundesliga", result.League);
        Assert.Equal("Football", result.LeagueGroup);
        Assert.Equal("3.0", result.Spread);
        Assert.Equal("2000", result.MaxbetWD_HDP);
        Assert.Equal("8", result.WD_NoOfHDP);
        Assert.Equal("3000", result.MaxbetWK_HDP);
        Assert.Equal("15", result.WK_NoOfHDP);
        Assert.Equal("Insert Note", result.LstNote);
        Assert.Equal("Insert Remark 2", result.Remark2);
        Assert.False(result.HasTv);
        Assert.True(result.HasIpTv);
        Assert.Single(result.IpTv);
        Assert.Equal("IPTV3", result.IpTv[0].Value);
        Assert.Single(result.Round);
        Assert.Equal("Round3", result.Round[0].Value);
        Assert.Single(result.Division);
        Assert.Equal("Div3", result.Division[0].Value);
        Assert.Equal("Insert Remark", result.Remark);
        Assert.False(result.IsBlackList);
        Assert.Equal(1, result.NoOfferLive);
    }

    [Fact]
    public void MapToChangeModel_WithInsertRequest_WithNullProperties_ShouldHandleGracefully()
    {
        // Arrange
        var request = new InsertLeagueSettingRequest
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
        Assert.Null(result.IpTv);
        Assert.Null(result.Round);
        Assert.Null(result.Division);
        Assert.Null(result.Remark);
    }

    [Fact]
    public void MapToChangeModel_WithInsertRequest_WithEmptyLists_ShouldMapCorrectly()
    {
        // Arrange
        var request = new InsertLeagueSettingRequest
        {
            League = "Empty Lists League",
            LeagueGroup = "Test Group",
            IpTv = new List<Condition>(),
            Round = new List<Condition>(),
            Division = new List<Condition>()
        };

        // Act
        var result = LoggerServiceImp.MapToChangeModel(request);

        // Assert
        Assert.Equal("Empty Lists League", result.League);
        Assert.NotNull(result.IpTv);
        Assert.Empty(result.IpTv);
        Assert.NotNull(result.Round);
        Assert.Empty(result.Round);
        Assert.NotNull(result.Division);
        Assert.Empty(result.Division);
    }

    [Fact]
    public void MapToChangeModel_WithInsertRequest_WithMultipleConditions_ShouldMapAll()
    {
        // Arrange
        var request = new InsertLeagueSettingRequest
        {
            League = "Multi Condition League",
            LeagueGroup = "Test Group",
            IpTv = new List<Condition>
            {
                new Condition("Contains", "IPTV1"),
                new Condition("Equals", "IPTV2")
            },
            Round = new List<Condition>
            {
                new Condition("Contains", "Round1"),
                new Condition("Contains", "Round2"),
                new Condition("Equals", "Round3")
            },
            Division = new List<Condition>
            {
                new Condition("Equals", "DivA"),
                new Condition("Contains", "DivB")
            }
        };

        // Act
        var result = LoggerServiceImp.MapToChangeModel(request);

        // Assert
        Assert.Equal(2, result.IpTv.Count);
        Assert.Equal("IPTV1", result.IpTv[0].Value);
        Assert.Equal("IPTV2", result.IpTv[1].Value);
        Assert.Equal(3, result.Round.Count);
        Assert.Equal("Round1", result.Round[0].Value);
        Assert.Equal("Round2", result.Round[1].Value);
        Assert.Equal("Round3", result.Round[2].Value);
        Assert.Equal(2, result.Division.Count);
        Assert.Equal("DivA", result.Division[0].Value);
        Assert.Equal("DivB", result.Division[1].Value);
    }

    [Fact]
    public async Task LogSetting_WithMultipleChanges_ShouldSerializeAllChangesToJson()
    {
        // Arrange
        var changeService = Substitute.For<ILeagueSettingChangeService>();
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();

        var logService = new LoggerServiceImp(changeService, repository, session);

        var currentSetting = new LeagueSettingChangeModel { League = "Old League", Spread = "1.0" };
        var newSetting = new LeagueSettingChangeModel { League = "New League", Spread = "2.0" };
        var request = new LogRequest("Multiple changes", 999, currentSetting, newSetting);

        var logDetails = new List<LogDetail>
        {
            new LogDetail { Field = "League Name", OldValue = "Old League", NewValue = "New League" },
            new LogDetail { Field = "FT Spread", OldValue = "1.0", NewValue = "2.0" }
        };

        changeService.LogProcessingChanges(currentSetting, newSetting).Returns(logDetails);
        session.UserName.Returns("admin");

        // Act
        await logService.LogSetting(request);

        // Assert
        await repository.Received(1).ExecuteNonQueryCommand(Arg.Is<InsertLogCriteria>(criteria =>
            criteria.Content.Contains("League Name") &&
            criteria.Content.Contains("Old League") &&
            criteria.Content.Contains("New League") &&
            criteria.Content.Contains("FT Spread") &&
            criteria.Content.Contains("1.0") &&
            criteria.Content.Contains("2.0")
        ));
    }

    #region LeagueSettingInfoSender Computed Properties Tests

    [Fact]
    public void MapToChangeModel_WithLeagueSettingInfoSender_WithSpreadValues_ShouldFormatCorrectly()
    {
        // Arrange
        var leagueSetting = new LeagueSettingInfoSender
        {
            League = "Test League",
            LeagueGroup = "Test Group",
            Spread_HDP_Value = 0.75m,
            Spread_OU_Value = 2.5m,
            FullConditionDescription = JsonConvert.SerializeObject(new LeagueSettingCondition())
        };

        // Act
        var result = LoggerServiceImp.MapToChangeModel(leagueSetting);

        // Assert
        Assert.Equal("75%-250%", result.Spread);
    }

    [Fact]
    public void MapToChangeModel_WithLeagueSettingInfoSender_WithNullSpreadValues_ShouldReturnNull()
    {
        // Arrange
        var leagueSetting = new LeagueSettingInfoSender
        {
            League = "Test League",
            LeagueGroup = "Test Group",
            Spread_HDP_Value = null,
            Spread_OU_Value = null,
            FullConditionDescription = JsonConvert.SerializeObject(new LeagueSettingCondition())
        };

        // Act
        var result = LoggerServiceImp.MapToChangeModel(leagueSetting);

        // Assert
        Assert.Null(result.Spread);
    }

    [Fact]
    public void MapToChangeModel_WithLeagueSettingInfoSender_WithIntegerSpreadValues_ShouldFormatWithoutDecimal()
    {
        // Arrange
        var leagueSetting = new LeagueSettingInfoSender
        {
            League = "Test League",
            LeagueGroup = "Test Group",
            Spread_HDP_Value = 1.0m,
            Spread_OU_Value = 2.0m,
            FullConditionDescription = JsonConvert.SerializeObject(new LeagueSettingCondition())
        };

        // Act
        var result = LoggerServiceImp.MapToChangeModel(leagueSetting);

        // Assert
        Assert.Equal("100%-200%", result.Spread);
    }

    [Fact]
    public void MapToChangeModel_WithLeagueSettingInfoSender_WithMaxbetValues_ShouldFormatInK()
    {
        // Arrange
        var leagueSetting = new LeagueSettingInfoSender
        {
            League = "Test League",
            LeagueGroup = "Test Group",
            MaxbetWD_HDP_Value = 5000,
            MaxbetWK_HDP_Value = 10000,
            MaxbetWD_HDP_Note = "",
            FullConditionDescription = JsonConvert.SerializeObject(new LeagueSettingCondition())
        };

        // Act
        var result = LoggerServiceImp.MapToChangeModel(leagueSetting);

        // Assert
        Assert.Equal("5K", result.MaxbetWD_HDP);
        Assert.Equal("10K", result.MaxbetWK_HDP);
    }

    [Fact]
    public void MapToChangeModel_WithLeagueSettingInfoSender_WithNullMaxbetValues_ShouldUseNote()
    {
        // Arrange
        var leagueSetting = new LeagueSettingInfoSender();
        leagueSetting.League = "Test League";
        leagueSetting.LeagueGroup = "Test Group";
        leagueSetting.MaxbetWD_HDP_Value = null;
        leagueSetting.MaxbetWK_HDP_Value = null;
        leagueSetting.MaxbetWD_HDP_Note = "Standard";
        leagueSetting.MaxbetWK_HDP_Note = "Standard";
        leagueSetting.FullConditionDescription = JsonConvert.SerializeObject(new LeagueSettingCondition());

        // Act
        var result = LoggerServiceImp.MapToChangeModel(leagueSetting);

        // Assert
        Assert.Equal("Standard", result.MaxbetWD_HDP);
        Assert.Equal("Standard", result.MaxbetWK_HDP);
    }

    [Fact]
    public void MapToChangeModel_WithLeagueSettingInfoSender_WithNoOfHdpValues_ShouldMapCorrectly()
    {
        // Arrange
        var leagueSetting = new LeagueSettingInfoSender
        {
            League = "Test League",
            LeagueGroup = "Test Group",
            WD_NoOfHDP_Value = "5",
            WK_NoOfHDP_Value = "10",
            FullConditionDescription = JsonConvert.SerializeObject(new LeagueSettingCondition())
        };

        // Act
        var result = LoggerServiceImp.MapToChangeModel(leagueSetting);

        // Assert
        Assert.Equal("5", result.WD_NoOfHDP);
        Assert.Equal("10", result.WK_NoOfHDP);
    }

    [Fact]
    public void MapToChangeModel_WithLeagueSettingInfoSender_WithEmptyNoOfHdpValues_ShouldReturnEmpty()
    {
        // Arrange
        var leagueSetting = new LeagueSettingInfoSender
        {
            League = "Test League",
            LeagueGroup = "Test Group",
            WD_NoOfHDP_Value = "",
            WK_NoOfHDP_Value = null,
            FullConditionDescription = JsonConvert.SerializeObject(new LeagueSettingCondition())
        };

        // Act
        var result = LoggerServiceImp.MapToChangeModel(leagueSetting);

        // Assert
        Assert.Equal("", result.WD_NoOfHDP);
        Assert.Equal("", result.WK_NoOfHDP);
    }

    [Fact]
    public void MapToChangeModel_WithLeagueSettingInfoSender_WithEmptyConditionDescription_ShouldUseDefaultCondition()
    {
        // Arrange
        var leagueSetting = new LeagueSettingInfoSender
        {
            League = "Test League",
            LeagueGroup = "Test Group",
            FullConditionDescription = ""
        };

        // Act
        var result = LoggerServiceImp.MapToChangeModel(leagueSetting);

        // Assert
        Assert.False(result.HasTv);
        Assert.False(result.HasIpTv);
        Assert.Empty(result.IpTv);
        Assert.Empty(result.Round);
        Assert.Empty(result.Division);
    }

    [Fact]
    public void MapToChangeModel_WithLeagueSettingInfoSender_WithNullConditionDescription_ShouldUseDefaultCondition()
    {
        // Arrange
        var leagueSetting = new LeagueSettingInfoSender
        {
            League = "Test League",
            LeagueGroup = "Test Group",
            FullConditionDescription = null
        };

        // Act
        var result = LoggerServiceImp.MapToChangeModel(leagueSetting);

        // Assert
        Assert.False(result.HasTv);
        Assert.False(result.HasIpTv);
        Assert.Empty(result.IpTv);
        Assert.Empty(result.Round);
        Assert.Empty(result.Division);
    }

    [Fact]
    public void MapToChangeModel_WithLeagueSettingInfoSender_WithComplexSpreadValues_ShouldFormatCorrectly()
    {
        // Arrange
        var leagueSetting = new LeagueSettingInfoSender
        {
            League = "Test League",
            LeagueGroup = "Test Group",
            Spread_HDP_Value = 0.25m,
            Spread_OU_Value = 1.75m,
            FullConditionDescription = JsonConvert.SerializeObject(new LeagueSettingCondition())
        };

        // Act
        var result = LoggerServiceImp.MapToChangeModel(leagueSetting);

        // Assert
        Assert.Equal("25%-175%", result.Spread);
    }

    [Fact]
    public void MapToChangeModel_WithLeagueSettingInfoSender_WithDecimalMaxbetValues_ShouldFormatCorrectly()
    {
        // Arrange
        var leagueSetting = new LeagueSettingInfoSender
        {
            League = "Test League",
            LeagueGroup = "Test Group",
            MaxbetWD_HDP_Value = 7500,
            MaxbetWK_HDP_Value = 12500,
            MaxbetWD_HDP_Note = "",
            FullConditionDescription = JsonConvert.SerializeObject(new LeagueSettingCondition())
        };

        // Act
        var result = LoggerServiceImp.MapToChangeModel(leagueSetting);

        // Assert
        Assert.Equal("7.5K", result.MaxbetWD_HDP);
        Assert.Equal("12.5K", result.MaxbetWK_HDP);
    }

    [Fact]
    public void MapToChangeModel_WithLeagueSettingInfoSender_WithAllFields_ShouldMapCompletely()
    {
        // Arrange
        var leagueSetting = new LeagueSettingInfoSender
        {
            League = "Premier League",
            LeagueGroup = "England",
            LstNote = "Important Note",
            Remark2 = "Additional Remark",
            IsBlackList = true,
            NoOfferLive = 0,
            Spread_HDP_Value = 1.5m,
            Spread_OU_Value = 3.5m,
            MaxbetWD_HDP_Value = 15000,
            MaxbetWK_HDP_Value = 25000,
            MaxbetWD_HDP_Note = "",
            WD_NoOfHDP_Value = "8",
            WK_NoOfHDP_Value = "12",
            FullConditionDescription = JsonConvert.SerializeObject(new LeagueSettingCondition
            {
                HasTv = true,
                HasIpTv = true,
                IpTv = new List<Condition> { new Condition("Equals", "SkySports") },
                Round = new List<Condition> { new Condition("Contains", "Final") },
                Division = new List<Condition> { new Condition("Equals", "Premier") },
                Remark = "Top League"
            })
        };

        // Act
        var result = LoggerServiceImp.MapToChangeModel(leagueSetting);

        // Assert
        Assert.Equal("Premier League", result.League);
        Assert.Equal("England", result.LeagueGroup);
        Assert.Equal("150%-350%", result.Spread);
        Assert.Equal("15K", result.MaxbetWD_HDP);
        Assert.Equal("25K", result.MaxbetWK_HDP);
        Assert.Equal("8", result.WD_NoOfHDP);
        Assert.Equal("12", result.WK_NoOfHDP);
        Assert.Equal("Important Note", result.LstNote);
        Assert.Equal("Additional Remark", result.Remark2);
        Assert.True(result.HasTv);
        Assert.True(result.HasIpTv);
        Assert.Single(result.IpTv);
        Assert.Equal("SkySports", result.IpTv[0].Value);
        Assert.Single(result.Round);
        Assert.Equal("Final", result.Round[0].Value);
        Assert.Single(result.Division);
        Assert.Equal("Premier", result.Division[0].Value);
        Assert.Equal("Top League", result.Remark);
        Assert.True(result.IsBlackList);
        Assert.Equal(0, result.NoOfferLive);
    }

    #endregion

    #region Edge Cases for Repository and Session

    [Fact]
    public async Task LogSetting_WithDifferentUserNames_ShouldLogCorrectUser()
    {
        // Arrange
        var changeService = Substitute.For<ILeagueSettingChangeService>();
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();

        var logService = new LoggerServiceImp(changeService, repository, session);

        var currentSetting = new LeagueSettingChangeModel();
        var newSetting = new LeagueSettingChangeModel();
        var request = new LogRequest("Test Action", 1, currentSetting, newSetting);

        changeService.LogProcessingChanges(Arg.Any<LeagueSettingChangeModel>(), Arg.Any<LeagueSettingChangeModel>())
            .Returns(new List<LogDetail>());

        // Test different usernames
        session.UserName.Returns("admin");
        await logService.LogSetting(request);
        await repository.Received().ExecuteNonQueryCommand(Arg.Is<InsertLogCriteria>(c => c.ModifiedBy == "admin"));

        repository.ClearReceivedCalls();
        session.UserName.Returns("user123");
        await logService.LogSetting(request);
        await repository.Received().ExecuteNonQueryCommand(Arg.Is<InsertLogCriteria>(c => c.ModifiedBy == "user123"));
    }

    [Fact]
    public async Task LogSetting_WithLargeSettingId_ShouldHandleCorrectly()
    {
        // Arrange
        var changeService = Substitute.For<ILeagueSettingChangeService>();
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();

        var logService = new LoggerServiceImp(changeService, repository, session);

        var currentSetting = new LeagueSettingChangeModel();
        var newSetting = new LeagueSettingChangeModel();
        var request = new LogRequest("Test", int.MaxValue, currentSetting, newSetting);

        changeService.LogProcessingChanges(Arg.Any<LeagueSettingChangeModel>(), Arg.Any<LeagueSettingChangeModel>())
            .Returns(new List<LogDetail>());
        session.UserName.Returns("testuser");

        // Act
        await logService.LogSetting(request);

        // Assert
        await repository.Received(1).ExecuteNonQueryCommand(Arg.Is<InsertLogCriteria>(criteria =>
            criteria.SettingId == int.MaxValue
        ));
    }

    [Fact]
    public async Task LogSetting_WithSpecialCharactersInAction_ShouldHandleCorrectly()
    {
        // Arrange
        var changeService = Substitute.For<ILeagueSettingChangeService>();
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();

        var logService = new LoggerServiceImp(changeService, repository, session);

        var currentSetting = new LeagueSettingChangeModel();
        var newSetting = new LeagueSettingChangeModel();
        var request = new LogRequest("Updated: League \"Test\" & Settings <Important>", 1, currentSetting, newSetting);

        changeService.LogProcessingChanges(Arg.Any<LeagueSettingChangeModel>(), Arg.Any<LeagueSettingChangeModel>())
            .Returns(new List<LogDetail>());
        session.UserName.Returns("testuser");

        // Act
        await logService.LogSetting(request);

        // Assert
        await repository.Received(1).ExecuteNonQueryCommand(Arg.Is<InsertLogCriteria>(criteria =>
            criteria.Action == "Updated: League \"Test\" & Settings <Important>"
        ));
    }

    [Fact]
    public async Task LogSetting_WithVeryLongActionString_ShouldHandleCorrectly()
    {
        // Arrange
        var changeService = Substitute.For<ILeagueSettingChangeService>();
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();

        var logService = new LoggerServiceImp(changeService, repository, session);

        var currentSetting = new LeagueSettingChangeModel();
        var newSetting = new LeagueSettingChangeModel();
        var longAction = new string('A', 1000);
        var request = new LogRequest(longAction, 1, currentSetting, newSetting);

        changeService.LogProcessingChanges(Arg.Any<LeagueSettingChangeModel>(), Arg.Any<LeagueSettingChangeModel>())
            .Returns(new List<LogDetail>());
        session.UserName.Returns("testuser");

        // Act
        await logService.LogSetting(request);

        // Assert
        await repository.Received(1).ExecuteNonQueryCommand(Arg.Is<InsertLogCriteria>(criteria =>
            criteria.Action == longAction
        ));
    }

    #endregion

    #region Update and Insert Request Edge Cases

    [Fact]
    public void MapToChangeModel_WithUpdateRequest_WithAllEmptyStrings_ShouldMapCorrectly()
    {
        // Arrange
        var request = new UpdateLeagueSettingRequest
        {
            League = "",
            LeagueGroup = "",
            Spread = "",
            MaxbetWD_HDP = "",
            WD_NoOfHDP = "",
            MaxbetWK_HDP = "",
            WK_NoOfHDP = "",
            LstNote = "",
            Remark2 = "",
            Remark = "",
            IpTv = new List<Condition>(),
            Round = new List<Condition>(),
            Division = new List<Condition>()
        };

        // Act
        var result = LoggerServiceImp.MapToChangeModel(request);

        // Assert
        Assert.Equal("", result.League);
        Assert.Equal("", result.LeagueGroup);
        Assert.Equal("", result.Spread);
        Assert.Empty(result.IpTv);
        Assert.Empty(result.Round);
        Assert.Empty(result.Division);
    }

    [Fact]
    public void MapToChangeModel_WithInsertRequest_WithWhitespaceStrings_ShouldPreserveWhitespace()
    {
        // Arrange
        var request = new InsertLeagueSettingRequest
        {
            League = "   ",
            LeagueGroup = " Test ",
            LstNote = "\t\n",
            Remark = "  "
        };

        // Act
        var result = LoggerServiceImp.MapToChangeModel(request);

        // Assert
        Assert.Equal("   ", result.League);
        Assert.Equal(" Test ", result.LeagueGroup);
        Assert.Equal("\t\n", result.LstNote);
        Assert.Equal("  ", result.Remark);
    }



    [Fact]
    public async Task LogSetting_WithNullSessionUserName_ShouldHandleGracefully()
    {
        // Arrange
        var changeService = Substitute.For<ILeagueSettingChangeService>();
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();

        var logService = new LoggerServiceImp(changeService, repository, session);

        var currentSetting = new LeagueSettingChangeModel { League = "Test League" };
        var newSetting = new LeagueSettingChangeModel { League = "Updated League" };
        var request = new LogRequest("Test Action", 123, currentSetting, newSetting);

        changeService.LogProcessingChanges(currentSetting, newSetting).Returns(new List<LogDetail>());
        session.UserName.Returns((string)null);

        // Act
        await logService.LogSetting(request);

        // Assert
        await repository.Received(1).ExecuteNonQueryCommand(Arg.Is<InsertLogCriteria>(criteria =>
            criteria.ModifiedBy == null
        ));
    }

    [Fact]
    public void MapToChangeModel_WithUpdateRequest_WithBooleanEdgeCases_ShouldMapCorrectly()
    {
        // Arrange - All true
        var requestAllTrue = new UpdateLeagueSettingRequest
        {
            HasTv = true,
            HasIpTv = true,
            IsBlackList = true,
            NoOfferLive = 1
        };

        // Act
        var resultAllTrue = LoggerServiceImp.MapToChangeModel(requestAllTrue);

        // Assert
        Assert.True(resultAllTrue.HasTv);
        Assert.True(resultAllTrue.HasIpTv);
        Assert.True(resultAllTrue.IsBlackList);
        Assert.Equal(1, resultAllTrue.NoOfferLive);

        // Arrange - All false
        var requestAllFalse = new UpdateLeagueSettingRequest
        {
            HasTv = false,
            HasIpTv = false,
            IsBlackList = false,
            NoOfferLive = 0
        };

        // Act
        var resultAllFalse = LoggerServiceImp.MapToChangeModel(requestAllFalse);

        // Assert
        Assert.False(resultAllFalse.HasTv);
        Assert.False(resultAllFalse.HasIpTv);
        Assert.False(resultAllFalse.IsBlackList);
        Assert.Equal(0, resultAllFalse.NoOfferLive);
    }

    #endregion

    #region Exception Handling Tests

    [Fact]
    public async Task LogSetting_WithRepositoryException_ShouldPropagateException()
    {
        // Arrange
        var changeService = Substitute.For<ILeagueSettingChangeService>();
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();

        var logService = new LoggerServiceImp(changeService, repository, session);

        var currentSetting = new LeagueSettingChangeModel { League = "Test League" };
        var newSetting = new LeagueSettingChangeModel { League = "Updated League" };
        var request = new LogRequest("Test Action", 123, currentSetting, newSetting);

        var expectedException = new Exception("Repository error");
        changeService.LogProcessingChanges(currentSetting, newSetting).Returns(new List<LogDetail>());
        repository.ExecuteNonQueryCommand(Arg.Any<InsertLogCriteria>()).Returns(Task.FromException(expectedException));
        session.UserName.Returns("testuser");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => logService.LogSetting(request));
        Assert.Equal(expectedException, exception);
    }

    [Fact]
    public async Task LogSetting_WithChangeServiceException_ShouldPropagateException()
    {
        // Arrange
        var changeService = Substitute.For<ILeagueSettingChangeService>();
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();

        var logService = new LoggerServiceImp(changeService, repository, session);

        var currentSetting = new LeagueSettingChangeModel { League = "Test League" };
        var newSetting = new LeagueSettingChangeModel { League = "Updated League" };
        var request = new LogRequest("Test Action", 123, currentSetting, newSetting);

        var expectedException = new Exception("Change service error");
        changeService.LogProcessingChanges(Arg.Any<LeagueSettingChangeModel>(), Arg.Any<LeagueSettingChangeModel>()).Returns(_ => throw expectedException);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => logService.LogSetting(request));
        Assert.Equal(expectedException, exception);
    }

    [Fact]
    public async Task LogSetting_WithSessionException_ShouldPropagateException()
    {
        // Arrange
        var changeService = Substitute.For<ILeagueSettingChangeService>();
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();

        var logService = new LoggerServiceImp(changeService, repository, session);

        var currentSetting = new LeagueSettingChangeModel { League = "Test League" };
        var newSetting = new LeagueSettingChangeModel { League = "Updated League" };
        var request = new LogRequest("Test Action", 123, currentSetting, newSetting);

        var expectedException = new Exception("Session error");
        changeService.LogProcessingChanges(currentSetting, newSetting).Returns(new List<LogDetail>());
        session.UserName.Returns(_ => throw expectedException);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => logService.LogSetting(request));
        Assert.Equal(expectedException, exception);
    }

    #endregion
}
