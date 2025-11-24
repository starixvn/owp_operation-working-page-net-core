using System;
using System.Collections.Generic;
using OWP.EventPicking.Api.Models;
using Xunit;

namespace OWP.EventPicking.UnitTests.Models;

public class EventPickingModelUnitTests
{
    [Fact]
    public void EventPickingModel_ShouldHaveCorrectProperties()
    {
        // Arrange
        var model = new EventPickingModel
        {
            InfoSiteMatchId = 123,
            MatchNo = "Test Match",
            EventDateTime = DateTime.Now,
            OWPLeagueId = 456,
            HomeName = "Home Team",
            AwayName = "Away Team",
            LeagueName = "Test League",
            Provider = "Test Provider",
            MaxbetHdp = 1000,
            IsNoOffer = false
        };

        // Assert
        Assert.Equal(123, model.InfoSiteMatchId);
        Assert.Equal("Test Match", model.MatchNo);
        Assert.Equal(456, model.OWPLeagueId);
        Assert.Equal("Home Team", model.HomeName);
        Assert.Equal("Away Team", model.AwayName);
        Assert.Equal("Test League", model.LeagueName);
        Assert.Equal("Test Provider", model.Provider);
        Assert.Equal(1000, model.MaxbetHdp);
        Assert.False(model.IsNoOffer);
    }

    [Fact]
    public void EventPickingModel_ShouldHandleNullValues()
    {
        // Arrange
        var model = new EventPickingModel();

        // Assert
        Assert.Equal(0, model.InfoSiteMatchId);
        Assert.Null(model.MatchNo);
        Assert.Equal(default(DateTime), model.EventDateTime);
        Assert.Equal(0, model.OWPLeagueId);
        Assert.Null(model.HomeName);
        Assert.Null(model.AwayName);
        Assert.Null(model.LeagueName);
        Assert.Null(model.Provider);
        Assert.Null(model.MaxbetHdp);
        Assert.Null(model.IsNoOffer);
    }

    [Fact]
    public void EventPickingModel_ShouldInheritFromBaseClasses()
    {
        // Arrange
        var model = new EventPickingModel
        {
            InfoSiteMatchId = 789,
            MatchNo = "Inheritance Test",
            EventDateTime = new DateTime(2024, 1, 1),
            OWPLeagueId = 101,
            HomeName = "Home",
            AwayName = "Away",
            LeagueName = "Inheritance League",
            Provider = "Inheritance Provider",
            MaxbetHdp = 2000,
            IsNoOffer = true
        };

        // Assert
        Assert.Equal(789, model.InfoSiteMatchId);
        Assert.Equal("Inheritance Test", model.MatchNo);
        Assert.Equal(new DateTime(2024, 1, 1), model.EventDateTime);
        Assert.Equal(101, model.OWPLeagueId);
        Assert.Equal("Home", model.HomeName);
        Assert.Equal("Away", model.AwayName);
        Assert.Equal("Inheritance League", model.LeagueName);
        Assert.Equal("Inheritance Provider", model.Provider);
        Assert.Equal(2000, model.MaxbetHdp);
        Assert.True(model.IsNoOffer);
    }

    [Fact]
    public void EventPickingModel_ShouldHandleMaxbetAndSpreadProperties()
    {
        // Arrange
        var model = new EventPickingModel
        {
            MaxbetHdp = 5000,
            SpreadHdp = 0.05m,
            SpreadOu = 0.10m
        };

        // Assert
        Assert.Equal(5000, model.MaxbetHdp);
        Assert.Equal(0.05m, model.SpreadHdp);
        Assert.Equal(0.10m, model.SpreadOu);
    }

    [Fact]
    public void EventPickingModel_ShouldHandleBooleanProperties()
    {
        // Arrange
        var model = new EventPickingModel
        {
            IsNoOffer = true,
            IsTestMatch = true,
            IsDeleted = false,
            IsCheck = true
        };

        // Assert
        Assert.True(model.IsNoOffer);
        Assert.True(model.IsTestMatch);
        Assert.False(model.IsDeleted);
        Assert.True(model.IsCheck);
    }

    [Fact]
    public void EventPickingModel_ShouldHandleDateTimeProperties()
    {
        // Arrange
        var testDate = new DateTime(2024, 12, 25, 15, 30, 0);
        var model = new EventPickingModel
        {
            EventDateTime = testDate
        };

        // Assert
        Assert.Equal(testDate, model.EventDateTime);
    }

    [Fact]
    public void EventPickingModel_ShouldHandleStringProperties()
    {
        // Arrange
        var model = new EventPickingModel
        {
            MatchNo = "String Test",
            HomeName = "Home String",
            AwayName = "Away String",
            LeagueName = "League String",
            Provider = "Provider String"
        };

        // Assert
        Assert.Equal("String Test", model.MatchNo);
        Assert.Equal("Home String", model.HomeName);
        Assert.Equal("Away String", model.AwayName);
        Assert.Equal("League String", model.LeagueName);
        Assert.Equal("Provider String", model.Provider);
    }

    [Fact]
    public void EventPickingModel_ShouldHandleNumericProperties()
    {
        // Arrange
        var model = new EventPickingModel
        {
            InfoSiteMatchId = 999,
            OWPLeagueId = 888,
            MaxbetHdp = 7777
        };

        // Assert
        Assert.Equal(999, model.InfoSiteMatchId);
        Assert.Equal(888, model.OWPLeagueId);
        Assert.Equal(7777, model.MaxbetHdp);
    }

    [Fact]
    public void EventPickingModel_ShouldHandleNullNumericProperties()
    {
        // Arrange
        var model = new EventPickingModel
        {
            MaxbetHdp = null,
            SpreadHdp = null,
            SpreadOu = null
        };

        // Assert
        Assert.Null(model.MaxbetHdp);
        Assert.Null(model.SpreadHdp);
        Assert.Null(model.SpreadOu);
    }

    [Fact]
    public void EventPickingModel_ShouldHandleEmptyStringProperties()
    {
        // Arrange
        var model = new EventPickingModel
        {
            MatchNo = "",
            HomeName = "",
            AwayName = "",
            LeagueName = "",
            Provider = ""
        };

        // Assert
        Assert.Equal("", model.MatchNo);
        Assert.Equal("", model.HomeName);
        Assert.Equal("", model.AwayName);
        Assert.Equal("", model.LeagueName);
        Assert.Equal("", model.Provider);
    }

    [Fact]
    public void EventPickingModel_ShouldHandleSpecialCharactersInStrings()
    {
        // Arrange
        var model = new EventPickingModel
        {
            MatchNo = "Test-Match_123",
            HomeName = "Home & Away",
            AwayName = "Away (Test)",
            LeagueName = "League: Test",
            Provider = "Provider@Test"
        };

        // Assert
        Assert.Equal("Test-Match_123", model.MatchNo);
        Assert.Equal("Home & Away", model.HomeName);
        Assert.Equal("Away (Test)", model.AwayName);
        Assert.Equal("League: Test", model.LeagueName);
        Assert.Equal("Provider@Test", model.Provider);
    }
}
