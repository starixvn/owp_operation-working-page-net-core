using OWP.EventPicking.Api.Helper;
using OWP.EventPicking.Api.Models;
using OWP.EventPicking.Api.Constant;
using OWP.Constants;
using Xunit;

namespace OWP.EventPicking.UnitTests.Helper;

public class MainMatchHelperUnitTest
{
    [Fact]
    public void Filter_ShouldRemoveChildLeaguesAndExcludedMatches()
    {
        // Arrange
        var matches = new List<EventPickingModel>
            {
                new EventPickingModel { LeagueName = "A CORNERS", HomeName = "Team A", RowColor = "red" },
                new EventPickingModel { LeagueName = "Main League", HomeName = "Team B", RowColor = "red" },
                new EventPickingModel { LeagueName = "B BOOKING", HomeName = "Excluded Team", RowColor = "red" }
            };

        // Act
        var result = MainMatchHelper.Filter(matches);

        // Assert
        Assert.Single(result);
        Assert.Equal("Team B", result[0].HomeName);
    }

    [Fact]
    public void Filter_ShouldRemoveCornersLeagues()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { LeagueName = "A CORNERS", HomeName = "Team A", RowColor = "red" },
            new EventPickingModel { LeagueName = "B CORNERS", HomeName = "Team B", RowColor = "red" },
            new EventPickingModel { LeagueName = "C CORNERS", HomeName = "Team C", RowColor = "red" },
            new EventPickingModel { LeagueName = "Main League", HomeName = "Team D", RowColor = "red" }
        };

        // Act
        var result = MainMatchHelper.Filter(matches);

        // Assert
        Assert.Single(result);
        Assert.Equal("Team D", result[0].HomeName);
    }

    [Fact]
    public void Filter_ShouldRemoveBookingLeagues()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { LeagueName = "A BOOKING", HomeName = "Team A", RowColor = "red" },
            new EventPickingModel { LeagueName = "B BOOKING", HomeName = "Team B", RowColor = "red" },
            new EventPickingModel { LeagueName = "C BOOKING", HomeName = "Team C", RowColor = "red" },
            new EventPickingModel { LeagueName = "Main League", HomeName = "Team D", RowColor = "red" }
        };

        // Act
        var result = MainMatchHelper.Filter(matches);

        // Assert
        Assert.Single(result);
        Assert.Equal("Team D", result[0].HomeName);
    }


    [Fact]
    public void Filter_ShouldHandleNullMatches()
    {
        // Arrange
        List<EventPickingModel> matches = null;

        // Act & Assert
        var exception = Record.Exception(() => MainMatchHelper.Filter(matches));
        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void RemoveTestingMatches_ShouldRemoveTestMatches()
    {
        // Arrange
        var matches = new List<EventPickingModel>
            {
                new EventPickingModel { IsTestMatch = true, HomeName = "Test Team" },
                new EventPickingModel { IsTestMatch = false, HomeName = "Real Team" }
            };

        // Act
        var result = MainMatchHelper.RemoveTestingMatches(matches);

        // Assert
        Assert.Single(result);
        Assert.Equal("Real Team", result.First().HomeName);
    }

    [Fact]
    public void RemoveTestingMatches_ShouldHandleNullMatches()
    {
        // Arrange
        List<EventPickingModel> matches = null;

        // Act & Assert
        var exception = Record.Exception(() => MainMatchHelper.RemoveTestingMatches(matches));
        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void Filter_ShouldHandleEmptyList()
    {
        // Arrange
        var matches = new List<EventPickingModel>();

        // Act
        var result = MainMatchHelper.Filter(matches);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void RemoveTestingMatches_ShouldHandleEmptyList()
    {
        // Arrange
        var matches = new List<EventPickingModel>();

        // Act
        var result = MainMatchHelper.RemoveTestingMatches(matches);

        // Assert
        Assert.Empty(result);
    }
}