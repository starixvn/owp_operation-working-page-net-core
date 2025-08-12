using System;
using OWP.EventPicking.Api.Models;
using Xunit;

namespace OWP.EventPicking.UnitTests.Models;

public class EventPickingModelUnitTests
{
    [Fact]
    public void EventPickingModel_ShouldHaveDefaultValues()
    {
        // Act
        var model = new EventPickingModel();

        // Assert
        Assert.Equal(0, model.Id);
        Assert.Null(model.Name);
        Assert.Equal(DateTime.MinValue, model.GameDate);
        Assert.Equal(0, model.LeagueId);
        Assert.Null(model.LeagueName);
        Assert.Equal(0, model.HomeTeamId);
        Assert.Null(model.HomeTeamName);
        Assert.Equal(0, model.AwayTeamId);
        Assert.Null(model.AwayTeamName);
        Assert.Equal(0, model.Status);
        Assert.Null(model.StatusName);
        Assert.Equal(0, model.ProviderId);
        Assert.Null(model.ProviderName);
        Assert.Equal(0, model.MaxbetSpread);
        Assert.False(model.IsConfirmed);
        Assert.Null(model.ConfirmedBy);
        Assert.Equal(DateTime.MinValue, model.ConfirmedAt);
        Assert.Null(model.CreatedBy);
        Assert.Equal(DateTime.MinValue, model.CreatedAt);
        Assert.Null(model.ModifiedBy);
        Assert.Equal(DateTime.MinValue, model.ModifiedAt);
    }

    [Fact]
    public void EventPickingModel_ShouldSetAndGetProperties()
    {
        // Arrange
        var model = new EventPickingModel();
        var testDate = DateTime.Now;
        var testUser = "testuser";

        // Act
        model.Id = 123;
        model.Name = "Test Match";
        model.GameDate = testDate;
        model.LeagueId = 456;
        model.LeagueName = "Test League";
        model.HomeTeamId = 789;
        model.HomeTeamName = "Home Team";
        model.AwayTeamId = 101;
        model.AwayTeamName = "Away Team";
        model.Status = 1;
        model.StatusName = "Active";
        model.ProviderId = 202;
        model.ProviderName = "Test Provider";
        model.MaxbetSpread = 1.5m;
        model.IsConfirmed = true;
        model.ConfirmedBy = testUser;
        model.ConfirmedAt = testDate;
        model.CreatedBy = testUser;
        model.CreatedAt = testDate;
        model.ModifiedBy = testUser;
        model.ModifiedAt = testDate;

        // Assert
        Assert.Equal(123, model.Id);
        Assert.Equal("Test Match", model.Name);
        Assert.Equal(testDate, model.GameDate);
        Assert.Equal(456, model.LeagueId);
        Assert.Equal("Test League", model.LeagueName);
        Assert.Equal(789, model.HomeTeamId);
        Assert.Equal("Home Team", model.HomeTeamName);
        Assert.Equal(101, model.AwayTeamId);
        Assert.Equal("Away Team", model.AwayTeamName);
        Assert.Equal(1, model.Status);
        Assert.Equal("Active", model.StatusName);
        Assert.Equal(202, model.ProviderId);
        Assert.Equal("Test Provider", model.ProviderName);
        Assert.Equal(1.5m, model.MaxbetSpread);
        Assert.True(model.IsConfirmed);
        Assert.Equal(testUser, model.ConfirmedBy);
        Assert.Equal(testDate, model.ConfirmedAt);
        Assert.Equal(testUser, model.CreatedBy);
        Assert.Equal(testDate, model.CreatedAt);
        Assert.Equal(testUser, model.ModifiedBy);
        Assert.Equal(testDate, model.ModifiedAt);
    }

    [Fact]
    public void EventPickingModel_ShouldHandleNullValues()
    {
        // Arrange
        var model = new EventPickingModel();

        // Act
        model.Name = null;
        model.LeagueName = null;
        model.HomeTeamName = null;
        model.AwayTeamName = null;
        model.StatusName = null;
        model.ProviderName = null;
        model.ConfirmedBy = null;
        model.CreatedBy = null;
        model.ModifiedBy = null;

        // Assert
        Assert.Null(model.Name);
        Assert.Null(model.LeagueName);
        Assert.Null(model.HomeTeamName);
        Assert.Null(model.AwayTeamName);
        Assert.Null(model.StatusName);
        Assert.Null(model.ProviderName);
        Assert.Null(model.ConfirmedBy);
        Assert.Null(model.CreatedBy);
        Assert.Null(model.ModifiedBy);
    }

    [Fact]
    public void EventPickingModel_ShouldHandleEdgeCaseValues()
    {
        // Arrange
        var model = new EventPickingModel();
        var minDate = DateTime.MinValue;
        var maxDate = DateTime.MaxValue;

        // Act
        model.Id = int.MaxValue;
        model.LeagueId = int.MinValue;
        model.HomeTeamId = 0;
        model.AwayTeamId = -1;
        model.Status = 999;
        model.ProviderId = 0;
        model.MaxbetSpread = decimal.MaxValue;
        model.GameDate = minDate;
        model.ConfirmedAt = maxDate;

        // Assert
        Assert.Equal(int.MaxValue, model.Id);
        Assert.Equal(int.MinValue, model.LeagueId);
        Assert.Equal(0, model.HomeTeamId);
        Assert.Equal(-1, model.AwayTeamId);
        Assert.Equal(999, model.Status);
        Assert.Equal(0, model.ProviderId);
        Assert.Equal(decimal.MaxValue, model.MaxbetSpread);
        Assert.Equal(minDate, model.GameDate);
        Assert.Equal(maxDate, model.ConfirmedAt);
    }

    [Fact]
    public void EventPickingModel_ShouldHandleDecimalPrecision()
    {
        // Arrange
        var model = new EventPickingModel();

        // Act
        model.MaxbetSpread = 1.123456789m;

        // Assert
        Assert.Equal(1.123456789m, model.MaxbetSpread);
    }

    [Fact]
    public void EventPickingModel_ShouldHandleBooleanValues()
    {
        // Arrange
        var model = new EventPickingModel();

        // Act
        model.IsConfirmed = true;

        // Assert
        Assert.True(model.IsConfirmed);

        // Act
        model.IsConfirmed = false;

        // Assert
        Assert.False(model.IsConfirmed);
    }

    [Fact]
    public void EventPickingModel_ShouldHandleDateTimeValues()
    {
        // Arrange
        var model = new EventPickingModel();
        var now = DateTime.Now;
        var utcNow = DateTime.UtcNow;

        // Act
        model.GameDate = now;
        model.CreatedAt = utcNow;

        // Assert
        Assert.Equal(now, model.GameDate);
        Assert.Equal(utcNow, model.CreatedAt);
    }

    [Fact]
    public void EventPickingModel_ShouldHandleStringValues()
    {
        // Arrange
        var model = new EventPickingModel();
        var emptyString = "";
        var longString = new string('A', 1000);

        // Act
        model.Name = emptyString;
        model.LeagueName = longString;

        // Assert
        Assert.Equal(emptyString, model.Name);
        Assert.Equal(longString, model.LeagueName);
    }

    [Fact]
    public void EventPickingModel_ShouldHandleSpecialCharacters()
    {
        // Arrange
        var model = new EventPickingModel();
        var specialChars = "!@#$%^&*()_+-=[]{}|;':\",./<>?";

        // Act
        model.Name = specialChars;

        // Assert
        Assert.Equal(specialChars, model.Name);
    }

    [Fact]
    public void EventPickingModel_ShouldHandleUnicodeCharacters()
    {
        // Arrange
        var model = new EventPickingModel();
        var unicodeString = "测试文本 with unicode 🚀";

        // Act
        model.Name = unicodeString;

        // Assert
        Assert.Equal(unicodeString, model.Name);
    }
}
