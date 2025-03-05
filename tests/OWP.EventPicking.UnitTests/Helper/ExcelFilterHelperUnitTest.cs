using OWP.EventPicking.Api.Helper;
using OWP.EventPicking.Api.Models;
using Xunit;

namespace OWP.EventPicking.UnitTests.Helper;
public class ExcelFilterHelperUnitTest
{
    [Fact]
    public void ExcludeChildMatch_ShouldRemoveChildMatches()
    {
        // Arrange
        var matches = new EventPickingResult
        {
            FilteredMatches =
            [
                new EventPickingModel { MinorBettypeId = 1, LeagueName = "League 1" },
                new EventPickingModel { MinorBettypeId = null, LeagueName = "League 2" },
                new EventPickingModel { MinorBettypeId = 1, LeagueName = "SABA League" }
            ]
        };

        // Act
        var result = ExcelFilterHelper.ExcludeChildMatch(matches);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.False(result[0].IsChildMatch);
    }

    [Fact]
    public void FilterLeagueName_ShouldExcludeMatchesWithSpecificText()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { LeagueName = "Premier League" },
            new EventPickingModel { LeagueName = "Championship" },
            new EventPickingModel { LeagueName = "La Liga" }
        };

        // Act
        var result = ExcelFilterHelper.FilterLeagueName(matches, "Premier");

        // Assert
        Assert.Equal(2, result.Count);
        Assert.DoesNotContain(result, m => m.LeagueName.Contains("Premier"));
    }

    [Fact]
    public void ExcludeChildMatch_ShouldHandleEmptyMatches()
    {
        // Arrange
        var matches = new EventPickingResult
        {
            FilteredMatches = new List<EventPickingModel>()
        };

        // Act
        var result = ExcelFilterHelper.ExcludeChildMatch(matches);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void FilterLeagueName_ShouldHandleEmptyMatches()
    {
        // Arrange
        var matches = new List<EventPickingModel>();

        // Act
        var result = ExcelFilterHelper.FilterLeagueName(matches, "Premier");

        // Assert
        Assert.Empty(result);
    }
}
