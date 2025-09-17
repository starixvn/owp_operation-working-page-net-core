using OWP.EventPicking.Api.Helper;
using OWP.EventPicking.Api.Models;
using Xunit;

namespace OWP.EventPicking.UnitTests.Helper;

public class SortEventPickingUnitTest
{
    [Fact]
    public void SortList_ShouldReturnEmptyList_WhenInputIsNull()
    {
        // Arrange
        List<EventProductCommon> matches = null;

        // Act & Assert
        var exception = Record.Exception(() => SortEventPicking.SortList(matches));
        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void SortList_ShouldReturnEmptyList_WhenInputIsEmpty()
    {
        // Arrange
        var matches = new List<EventProductCommon>();

        // Act
        var result = SortEventPicking.SortList(matches);

        // Assert
        Assert.Empty(result);
    }


    [Fact]
    public void SortList_ShouldSortByEventDateTime_WhenIsSabaIsSame()
    {
        // Arrange
        var now = DateTime.Now;
        var matches = new List<EventProductCommon>
        {
            new EventPickingModel { EventDateTime = now.AddHours(3), LeagueName = "League A", InfoSiteMatchId = 1 },
            new EventPickingModel { EventDateTime = now.AddHours(1), LeagueName = "League B", InfoSiteMatchId = 2 },
            new EventPickingModel { EventDateTime = now.AddHours(2), LeagueName = "League C", InfoSiteMatchId = 3 }
        };

        // Act
        var result = SortEventPicking.SortList(matches);

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal(now.AddHours(1), result[0].EventDateTime);
        Assert.Equal(now.AddHours(2), result[1].EventDateTime);
        Assert.Equal(now.AddHours(3), result[2].EventDateTime);
    }

    [Fact]
    public void SortList_ShouldSortByLeagueName_WhenIsSabaAndEventDateTimeAreSame()
    {
        // Arrange
        var now = DateTime.Now;
        var matches = new List<EventProductCommon>
        {
            new EventPickingModel { EventDateTime = now, LeagueName = "League C", InfoSiteMatchId = 1 },
            new EventPickingModel { EventDateTime = now, LeagueName = "League A", InfoSiteMatchId = 2 },
            new EventPickingModel { EventDateTime = now, LeagueName = "League B", InfoSiteMatchId = 3 }
        };

        // Act
        var result = SortEventPicking.SortList(matches);

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal("League A", result[0].LeagueName);
        Assert.Equal("League B", result[1].LeagueName);
        Assert.Equal("League C", result[2].LeagueName);
    }


    [Fact]
    public void SortList_ShouldHandleNullEventDateTime()
    {
        // Arrange
        var matches = new List<EventProductCommon>
        {
            new EventPickingModel { EventDateTime = DateTime.MinValue, LeagueName = "League A", InfoSiteMatchId = 1 },
            new EventPickingModel { EventDateTime = DateTime.Now, LeagueName = "League B", InfoSiteMatchId = 2 }
        };

        // Act
        var result = SortEventPicking.SortList(matches);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(DateTime.MinValue, result[0].EventDateTime);
        Assert.NotEqual(DateTime.MinValue, result[1].EventDateTime);
    }

    [Fact]
    public void SortList_ShouldHandleNullLeagueName()
    {
        // Arrange
        var now = DateTime.Now;
        var matches = new List<EventProductCommon>
        {
            new EventPickingModel { EventDateTime = now, LeagueName = null, InfoSiteMatchId = 1 },
            new EventPickingModel { EventDateTime = now, LeagueName = "League A", InfoSiteMatchId = 2 }
        };

        // Act
        var result = SortEventPicking.SortList(matches);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Null(result[0].LeagueName);
        Assert.Equal("League A", result[1].LeagueName);
    }

    [Fact]
    public void SortList_ShouldHandleNullInfoSiteMatchId()
    {
        // Arrange
        var now = DateTime.Now;
        var matches = new List<EventProductCommon>
        {
            new EventPickingModel { EventDateTime = now, LeagueName = "League A", InfoSiteMatchId = 0 },
            new EventPickingModel { EventDateTime = now, LeagueName = "League A", InfoSiteMatchId = 1 }
        };

        // Act
        var result = SortEventPicking.SortList(matches);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(0, result[0].InfoSiteMatchId);
        Assert.Equal(1, result[1].InfoSiteMatchId);
    }


    [Fact]
    public void SortList_ShouldHandleCaseInsensitiveLeagueNameSorting()
    {
        // Arrange
        var now = DateTime.Now;
        var matches = new List<EventProductCommon>
        {
            new EventPickingModel { EventDateTime = now, LeagueName = "league c", InfoSiteMatchId = 1 },
            new EventPickingModel { EventDateTime = now, LeagueName = "League A", InfoSiteMatchId = 2 },
            new EventPickingModel { EventDateTime = now, LeagueName = "LEAGUE B", InfoSiteMatchId = 3 }
        };

        // Act
        var result = SortEventPicking.SortList(matches);

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal("League A", result[0].LeagueName);
        Assert.Equal("LEAGUE B", result[1].LeagueName);
        Assert.Equal("league c", result[2].LeagueName);
    }

    [Fact]
    public void SortList_ShouldHandleEmptyLeagueName()
    {
        // Arrange
        var now = DateTime.Now;
        var matches = new List<EventProductCommon>
        {
            new EventPickingModel { EventDateTime = now, LeagueName = "", InfoSiteMatchId = 1 },
            new EventPickingModel { EventDateTime = now, LeagueName = "League A", InfoSiteMatchId = 2 }
        };

        // Act
        var result = SortEventPicking.SortList(matches);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("", result[0].LeagueName);
        Assert.Equal("League A", result[1].LeagueName);
    }

    [Fact]
    public void SortList_ShouldHandleWhitespaceLeagueName()
    {
        // Arrange
        var now = DateTime.Now;
        var matches = new List<EventProductCommon>
        {
            new EventPickingModel { EventDateTime = now, LeagueName = "   ", InfoSiteMatchId = 1 },
            new EventPickingModel { EventDateTime = now, LeagueName = "League A", InfoSiteMatchId = 2 }
        };

        // Act
        var result = SortEventPicking.SortList(matches);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("   ", result[0].LeagueName);
        Assert.Equal("League A", result[1].LeagueName);
    }

    [Fact]
    public void SortList_ShouldHandleNegativeInfoSiteMatchId()
    {
        // Arrange
        var now = DateTime.Now;
        var matches = new List<EventProductCommon>
        {
            new EventPickingModel { EventDateTime = now, LeagueName = "League A", InfoSiteMatchId = -1 },
            new EventPickingModel { EventDateTime = now, LeagueName = "League A", InfoSiteMatchId = 1 }
        };

        // Act
        var result = SortEventPicking.SortList(matches);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(-1, result[0].InfoSiteMatchId);
        Assert.Equal(1, result[1].InfoSiteMatchId);
    }

    [Fact]
    public void SortList_ShouldHandleZeroInfoSiteMatchId()
    {
        // Arrange
        var now = DateTime.Now;
        var matches = new List<EventProductCommon>
        {
            new EventPickingModel { EventDateTime = now, LeagueName = "League A", InfoSiteMatchId = 0 },
            new EventPickingModel { EventDateTime = now, LeagueName = "League A", InfoSiteMatchId = 1 }
        };

        // Act
        var result = SortEventPicking.SortList(matches);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(0, result[0].InfoSiteMatchId);
        Assert.Equal(1, result[1].InfoSiteMatchId);
    }




    [Fact]
    public void SortList_ShouldHandleDuplicateValues()
    {
        // Arrange
        var now = DateTime.Now;
        var matches = new List<EventProductCommon>
        {
            new EventPickingModel { EventDateTime = now, LeagueName = "League A", InfoSiteMatchId = 1 },
            new EventPickingModel { EventDateTime = now, LeagueName = "League A", InfoSiteMatchId = 1 },
            new EventPickingModel { EventDateTime = now, LeagueName = "League A", InfoSiteMatchId = 1 }
        };

        // Act
        var result = SortEventPicking.SortList(matches);

        // Assert
        Assert.Equal(3, result.Count);
        Assert.All(result, item =>
        {
            Assert.False(item.IsSaba);
            Assert.Equal(now, item.EventDateTime);
            Assert.Equal("League A", item.LeagueName);
            Assert.Equal(1, item.InfoSiteMatchId);
        });
    }
}