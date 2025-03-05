using OWP.EventPicking.Api.Helper;
using OWP.EventPicking.Api.Models;
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
                new EventPickingModel { LeagueName = "A CORNERS", HomeName = "Team A",RowColor = "red" },
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
    public void Filter_ShouldIncludeTestMatchesWhenSpecified()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { LeagueName = "Main League", HomeName = "Team A", IsTestMatch = true },
            new EventPickingModel { LeagueName = "Main League", HomeName = "Team B", IsTestMatch = false }
        };

        // Act
        var result = MainMatchHelper.Filter(matches, includeTestMatches: true);

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void RemoveTestingMatches_ShouldExcludeTestMatchesByDefault()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { LeagueName = "Main League", HomeName = "Team A", IsTestMatch = true },
            new EventPickingModel { LeagueName = "Main League", HomeName = "Team B", IsTestMatch = false }
        };

        // Act
        var result = MainMatchHelper.RemoveTestingMatches(matches);

        // Assert
        Assert.Single(result);
        Assert.Equal("Team B", result.First().HomeName);
    }

    [Fact]
    public void RemoveTestingMatches_ShouldIncludeTestMatchesWhenSpecified()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { LeagueName = "Main League", HomeName = "Team A", IsTestMatch = false },
            new EventPickingModel { LeagueName = "Main League", HomeName = "Team B", IsTestMatch = true }
        };

        // Act
        var result = MainMatchHelper.RemoveTestingMatches(matches, includeTestMatches: true);

        // Assert
        Assert.Equal(2, result.ToList().Count);
        Assert.Equal("Team A", result.First().HomeName);
    }
}