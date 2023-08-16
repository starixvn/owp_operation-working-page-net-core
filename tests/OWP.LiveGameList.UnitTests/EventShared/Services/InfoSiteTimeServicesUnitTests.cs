using NSubstitute;
using OWP.Event.EventShared.Models;
using OWP.Event.EventShared.Services;
using OWP.Models;
using OWP.Services;
using Xunit;

namespace OWP.Event.UnitTests.EventShared.Services;

using Xunit.Asserts.Compare;

public class InfoSiteTimeServicesUnitTests
{
    private readonly GetInfoSiteTimeService getInfoSiteTimeService;
    private readonly InfoSiteTimeServicesImp service;

    public InfoSiteTimeServicesUnitTests()
    {
        getInfoSiteTimeService = Substitute.For<GetInfoSiteTimeService>();

        service = new InfoSiteTimeServicesImp(getInfoSiteTimeService);
    }

    [Fact]
    public async Task GetInfositeInfo_ReturnInfositeInfoList()
    {
        const string parentMatchIds = "11,22";
        var infoSiteInfo = new List<InfoSiteTimeModel>
        {
            new() { AdminSiteMatchId = 11 }
        };
        getInfoSiteTimeService.GetInfoSiteTime(parentMatchIds).Returns(infoSiteInfo);

        var actual = await service.GetInfositeInfo(parentMatchIds);

        DeepAssert.Equal(infoSiteInfo, actual);
    }

    [Fact]
    public void BuildInfoSiteTimeAndAbsenceIcon_ReturnInfositeInfoList()
    {
        var matchInfo = new MatchInfo
        {
            OWPMatchId = 1,
            ParentMatchId = 22
        };

        var infoSiteInfo = new List<InfoSiteTimeModel>
        {
            new () { ParentMatchId = 22, KickOffTime = new TimeSpan(15,30,0) }
        };

        service.BuildInfoSiteTime(matchInfo, infoSiteInfo);

        Assert.Equal(new TimeSpan(15, 30, 0), matchInfo.InfoSiteTime);
        Assert.Equal("03:30 PM", matchInfo.InfoSiteTimeDisplay);
        Assert.False(matchInfo.IsShowAbsenceIcon);
    }

    [Fact]
    public void BuildInfoSiteTimeAndAbsenceIcon_NullInfosite_ReturnInfositeInfoList()
    {
        var matchInfo = new MatchInfo
        {
            OWPMatchId = 1,
            ParentMatchId = 22
        };

        var infoSiteInfo = new List<InfoSiteTimeModel>();

        service.BuildInfoSiteTime(matchInfo, infoSiteInfo);

        Assert.Null(matchInfo.InfoSiteTime);
        Assert.Null(matchInfo.InfoSiteTimeDisplay);
    }
}
