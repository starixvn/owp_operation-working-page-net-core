using OWP.EventPicking.Api.Helper;
using OWP.EventPicking.Api.Models;
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
}