using OWP.EventPicking.Api.Helper;
using OWP.EventPicking.Api.Models;
using OWP.EventPicking.Api.Constant;
using OWP.Constants;
using Xunit;

namespace OWP.EventPicking.UnitTests.Helper;

public class RAndDInfoHelperUnitTest
{
    [Fact]
    public void Build_ShouldAssignRAndDInfoToMatches()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { InfoSiteMatchId = 1 },
            new EventPickingModel { InfoSiteMatchId = 2 }
        };

        var rAndDInfos = new List<RAndDInfo>
        {
            new RAndDInfo { InfositeId = 1, RAndDUpdate = "Update1" },
            new RAndDInfo { InfositeId = 2, RAndDUpdate = "Update2" },
            new RAndDInfo { InfositeId = 1, RAndDUpdate = "Update3" }
        };

        // Act
        RAndDInfoHelper.Build(matches, rAndDInfos);

        // Assert
        Assert.Equal(2, matches[0].RAndDInfo.Count);
        Assert.Equal("Update1", matches[0].RAndDInfo[0].RAndDUpdate);
        Assert.Equal("Update3", matches[0].RAndDInfo[1].RAndDUpdate);
        Assert.Single(matches[1].RAndDInfo);
        Assert.Equal("Update2", matches[1].RAndDInfo[0].RAndDUpdate);
    }

    [Fact]
    public void Build_ShouldHandleEmptyRAndDInfos()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { InfoSiteMatchId = 1 },
            new EventPickingModel { InfoSiteMatchId = 2 }
        };

        var rAndDInfos = new List<RAndDInfo>();

        // Act
        RAndDInfoHelper.Build(matches, rAndDInfos);

        // Assert
        Assert.Empty(matches[0].RAndDInfo);
        Assert.Empty(matches[1].RAndDInfo);
    }

    [Fact]
    public void Build_ShouldHandleEmptyMatches()
    {
        // Arrange
        var matches = new List<EventPickingModel>();

        var rAndDInfos = new List<RAndDInfo>
        {
            new RAndDInfo { InfositeId = 1, RAndDUpdate = "Update1" },
            new RAndDInfo { InfositeId = 2, RAndDUpdate = "Update2" }
        };

        // Act
        RAndDInfoHelper.Build(matches, rAndDInfos);

        // Assert
        Assert.Empty(matches);
    }

    [Fact]
    public void Build_ShouldHandleNullMatches()
    {
        // Arrange
        List<EventPickingModel> matches = null;
        var rAndDInfos = new List<RAndDInfo>
        {
            new RAndDInfo { InfositeId = 1, RAndDUpdate = "Update1" }
        };

        // Act & Assert
        var exception = Record.Exception(() => RAndDInfoHelper.Build(matches, rAndDInfos));
        Assert.NotNull(exception);
        Assert.IsType<NullReferenceException>(exception);
    }

    [Fact]
    public void Build_ShouldHandleNullRAndDInfos()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { InfoSiteMatchId = 1 }
        };
        List<RAndDInfo> rAndDInfos = null;

        // Act & Assert
        var exception = Record.Exception(() => RAndDInfoHelper.Build(matches, rAndDInfos));
        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void Build_ShouldHandleNullInfoSiteMatchId()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { InfoSiteMatchId = 0 }
        };
        var rAndDInfos = new List<RAndDInfo>
        {
            new RAndDInfo { InfositeId = 1, RAndDUpdate = "Update1" }
        };

        // Act
        RAndDInfoHelper.Build(matches, rAndDInfos);

        // Assert
        Assert.Empty(matches[0].RAndDInfo);
    }

    [Fact]
    public void Build_ShouldHandleNullInfositeId()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { InfoSiteMatchId = 1 }
        };
        var rAndDInfos = new List<RAndDInfo>
        {
            new RAndDInfo { InfositeId = 0, RAndDUpdate = "Update1" }
        };

        // Act
        RAndDInfoHelper.Build(matches, rAndDInfos);

        // Assert
        Assert.Empty(matches[0].RAndDInfo);
    }

    [Fact]
    public void Build_ShouldHandleMismatchedIds()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { InfoSiteMatchId = 1 }
        };
        var rAndDInfos = new List<RAndDInfo>
        {
            new RAndDInfo { InfositeId = 2, RAndDUpdate = "Update1" }
        };

        // Act
        RAndDInfoHelper.Build(matches, rAndDInfos);

        // Assert
        Assert.Empty(matches[0].RAndDInfo);
    }

    [Fact]
    public void Build_ShouldHandleMultipleMatchesWithSameId()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { InfoSiteMatchId = 1 },
            new EventPickingModel { InfoSiteMatchId = 1 }
        };
        var rAndDInfos = new List<RAndDInfo>
        {
            new RAndDInfo { InfositeId = 1, RAndDUpdate = "Update1" }
        };

        // Act
        RAndDInfoHelper.Build(matches, rAndDInfos);

        // Assert
        Assert.Single(matches[0].RAndDInfo);
        Assert.Single(matches[1].RAndDInfo);
        Assert.Equal("Update1", matches[0].RAndDInfo[0].RAndDUpdate);
        Assert.Equal("Update1", matches[1].RAndDInfo[0].RAndDUpdate);
    }

    [Fact]
    public void Build_ShouldHandleMultipleRAndDInfosWithSameId()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { InfoSiteMatchId = 1 }
        };
        var rAndDInfos = new List<RAndDInfo>
        {
            new RAndDInfo { InfositeId = 1, RAndDUpdate = "Update1" },
            new RAndDInfo { InfositeId = 1, RAndDUpdate = "Update2" },
            new RAndDInfo { InfositeId = 1, RAndDUpdate = "Update3" }
        };

        // Act
        RAndDInfoHelper.Build(matches, rAndDInfos);

        // Assert
        Assert.Equal(3, matches[0].RAndDInfo.Count);
        Assert.Equal("Update1", matches[0].RAndDInfo[0].RAndDUpdate);
        Assert.Equal("Update2", matches[0].RAndDInfo[1].RAndDUpdate);
        Assert.Equal("Update3", matches[0].RAndDInfo[2].RAndDUpdate);
    }

    [Fact]
    public void Build_ShouldHandleComplexScenario()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { InfoSiteMatchId = 1 },
            new EventPickingModel { InfoSiteMatchId = 2 },
            new EventPickingModel { InfoSiteMatchId = 3 },
            new EventPickingModel { InfoSiteMatchId = 1 }
        };
        var rAndDInfos = new List<RAndDInfo>
        {
            new RAndDInfo { InfositeId = 1, RAndDUpdate = "Update1" },
            new RAndDInfo { InfositeId = 2, RAndDUpdate = "Update2" },
            new RAndDInfo { InfositeId = 1, RAndDUpdate = "Update3" },
            new RAndDInfo { InfositeId = 3, RAndDUpdate = "Update4" },
            new RAndDInfo { InfositeId = 2, RAndDUpdate = "Update5" }
        };

        // Act
        RAndDInfoHelper.Build(matches, rAndDInfos);

        // Assert
        Assert.Equal(2, matches[0].RAndDInfo.Count);
        Assert.Equal("Update1", matches[0].RAndDInfo[0].RAndDUpdate);
        Assert.Equal("Update3", matches[0].RAndDInfo[1].RAndDUpdate);

        Assert.Equal(2, matches[1].RAndDInfo.Count);
        Assert.Equal("Update2", matches[1].RAndDInfo[0].RAndDUpdate);
        Assert.Equal("Update5", matches[1].RAndDInfo[1].RAndDUpdate);

        Assert.Single(matches[2].RAndDInfo);
        Assert.Equal("Update4", matches[2].RAndDInfo[0].RAndDUpdate);

        Assert.Equal(2, matches[3].RAndDInfo.Count);
        Assert.Equal("Update1", matches[3].RAndDInfo[0].RAndDUpdate);
        Assert.Equal("Update3", matches[3].RAndDInfo[1].RAndDUpdate);
    }

    [Fact]
    public void Build_ShouldHandleZeroIds()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { InfoSiteMatchId = 0 }
        };
        var rAndDInfos = new List<RAndDInfo>
        {
            new RAndDInfo { InfositeId = 0, RAndDUpdate = "Update1" }
        };

        // Act
        RAndDInfoHelper.Build(matches, rAndDInfos);

        // Assert
        Assert.Single(matches[0].RAndDInfo);
        Assert.Equal("Update1", matches[0].RAndDInfo[0].RAndDUpdate);
    }

    [Fact]
    public void Build_ShouldHandleNegativeIds()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { InfoSiteMatchId = -1 }
        };
        var rAndDInfos = new List<RAndDInfo>
        {
            new RAndDInfo { InfositeId = -1, RAndDUpdate = "Update1" }
        };

        // Act
        RAndDInfoHelper.Build(matches, rAndDInfos);

        // Assert
        Assert.Single(matches[0].RAndDInfo);
        Assert.Equal("Update1", matches[0].RAndDInfo[0].RAndDUpdate);
    }

    [Fact]
    public void Build_ShouldHandleLargeIds()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { InfoSiteMatchId = int.MaxValue }
        };
        var rAndDInfos = new List<RAndDInfo>
        {
            new RAndDInfo { InfositeId = int.MaxValue, RAndDUpdate = "Update1" }
        };

        // Act
        RAndDInfoHelper.Build(matches, rAndDInfos);

        // Assert
        Assert.Single(matches[0].RAndDInfo);
        Assert.Equal("Update1", matches[0].RAndDInfo[0].RAndDUpdate);
    }

    [Fact]
    public void Build_ShouldHandleMinIds()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { InfoSiteMatchId = int.MinValue }
        };
        var rAndDInfos = new List<RAndDInfo>
        {
            new RAndDInfo { InfositeId = int.MinValue, RAndDUpdate = "Update1" }
        };

        // Act
        RAndDInfoHelper.Build(matches, rAndDInfos);

        // Assert
        Assert.Single(matches[0].RAndDInfo);
        Assert.Equal("Update1", matches[0].RAndDInfo[0].RAndDUpdate);
    }

    [Fact]
    public void Build_ShouldHandleNullRAndDUpdate()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { InfoSiteMatchId = 1 }
        };
        var rAndDInfos = new List<RAndDInfo>
        {
            new RAndDInfo { InfositeId = 1, RAndDUpdate = null }
        };

        // Act
        RAndDInfoHelper.Build(matches, rAndDInfos);

        // Assert
        Assert.Single(matches[0].RAndDInfo);
        Assert.Null(matches[0].RAndDInfo[0].RAndDUpdate);
    }

    [Fact]
    public void Build_ShouldHandleEmptyRAndDUpdate()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { InfoSiteMatchId = 1 }
        };
        var rAndDInfos = new List<RAndDInfo>
        {
            new RAndDInfo { InfositeId = 1, RAndDUpdate = "" }
        };

        // Act
        RAndDInfoHelper.Build(matches, rAndDInfos);

        // Assert
        Assert.Single(matches[0].RAndDInfo);
        Assert.Equal("", matches[0].RAndDInfo[0].RAndDUpdate);
    }

    [Fact]
    public void Build_ShouldHandleWhitespaceRAndDUpdate()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { InfoSiteMatchId = 1 }
        };
        var rAndDInfos = new List<RAndDInfo>
        {
            new RAndDInfo { InfositeId = 1, RAndDUpdate = "   " }
        };

        // Act
        RAndDInfoHelper.Build(matches, rAndDInfos);

        // Assert
        Assert.Single(matches[0].RAndDInfo);
        Assert.Equal("   ", matches[0].RAndDInfo[0].RAndDUpdate);
    }

    [Fact]
    public void Build_ShouldHandleVeryLongRAndDUpdate()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { InfoSiteMatchId = 1 }
        };
        var rAndDInfos = new List<RAndDInfo>
        {
            new RAndDInfo { InfositeId = 1, RAndDUpdate = new string('A', 10000) }
        };

        // Act
        RAndDInfoHelper.Build(matches, rAndDInfos);

        // Assert
        Assert.Single(matches[0].RAndDInfo);
        Assert.Equal(new string('A', 10000), matches[0].RAndDInfo[0].RAndDUpdate);
    }

    [Fact]
    public void Build_ShouldHandleSpecialCharactersInRAndDUpdate()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { InfoSiteMatchId = 1 }
        };
        var rAndDInfos = new List<RAndDInfo>
        {
            new RAndDInfo { InfositeId = 1, RAndDUpdate = "Update!@#$%^&*()_+-=[]{}|;':\",./<>?" }
        };

        // Act
        RAndDInfoHelper.Build(matches, rAndDInfos);

        // Assert
        Assert.Single(matches[0].RAndDInfo);
        Assert.Equal("Update!@#$%^&*()_+-=[]{}|;':\",./<>?", matches[0].RAndDInfo[0].RAndDUpdate);
    }

    [Fact]
    public void Build_ShouldHandleUnicodeCharactersInRAndDUpdate()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { InfoSiteMatchId = 1 }
        };
        var rAndDInfos = new List<RAndDInfo>
        {
            new RAndDInfo { InfositeId = 1, RAndDUpdate = "Update中文日本語한국어" }
        };

        // Act
        RAndDInfoHelper.Build(matches, rAndDInfos);

        // Assert
        Assert.Single(matches[0].RAndDInfo);
        Assert.Equal("Update中文日本語한국어", matches[0].RAndDInfo[0].RAndDUpdate);
    }

    [Fact]
    public void Build_ShouldHandleNewlinesInRAndDUpdate()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { InfoSiteMatchId = 1 }
        };
        var rAndDInfos = new List<RAndDInfo>
        {
            new RAndDInfo { InfositeId = 1, RAndDUpdate = "Update\nWith\nNewlines" }
        };

        // Act
        RAndDInfoHelper.Build(matches, rAndDInfos);

        // Assert
        Assert.Single(matches[0].RAndDInfo);
        Assert.Equal("Update\nWith\nNewlines", matches[0].RAndDInfo[0].RAndDUpdate);
    }

    [Fact]
    public void Build_ShouldHandleTabsInRAndDUpdate()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { InfoSiteMatchId = 1 }
        };
        var rAndDInfos = new List<RAndDInfo>
        {
            new RAndDInfo { InfositeId = 1, RAndDUpdate = "Update\tWith\tTabs" }
        };

        // Act
        RAndDInfoHelper.Build(matches, rAndDInfos);

        // Assert
        Assert.Single(matches[0].RAndDInfo);
        Assert.Equal("Update\tWith\tTabs", matches[0].RAndDInfo[0].RAndDUpdate);
    }

    [Fact]
    public void Build_ShouldHandleQuotesInRAndDUpdate()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { InfoSiteMatchId = 1 }
        };
        var rAndDInfos = new List<RAndDInfo>
        {
            new RAndDInfo { InfositeId = 1, RAndDUpdate = "\"Update\" 'With' Quotes" }
        };

        // Act
        RAndDInfoHelper.Build(matches, rAndDInfos);

        // Assert
        Assert.Single(matches[0].RAndDInfo);
        Assert.Equal("\"Update\" 'With' Quotes", matches[0].RAndDInfo[0].RAndDUpdate);
    }
}