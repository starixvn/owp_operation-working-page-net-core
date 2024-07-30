using OWP.EventPicking.Api.Helper;
using OWP.EventPicking.Api.Models;
using Xunit;

namespace OWP.EventPicking.UnitTests.Helper;

public class RAndDInfoHelperUnitTest
{
    [Fact]
    public void Build_ShouldPopulateRAndDInfos_ForMatchingInfositeId()
    {
        // Arrange
        var rAndDInfos = new List<RAndDInfo>
        {
            new() { InfositeId = 1, UpdateTypeId = 10 },
            new() { InfositeId = 1, UpdateTypeId = 20 },
            new() { InfositeId = 2, UpdateTypeId = 30 }
        };

        var matches = new List<EventPickingModel>
        {
            new() { InfoSiteMatchId = 1 },
            new() { InfoSiteMatchId = 2 }
        };

        // Act
        RAndDInfoHelper.Build(matches, rAndDInfos);

        // Assert
        Assert.Equal(2, matches[0].RAndDInfos.Count);
        Assert.Contains(rAndDInfos[0], matches[0].RAndDInfos);
        Assert.Contains(rAndDInfos[1], matches[0].RAndDInfos);

        Assert.Single(matches[1].RAndDInfos);
        Assert.Contains(rAndDInfos[2], matches[1].RAndDInfos);
    }
}