using NSubstitute;
using OWP.Event.EventShared;
using OWP.Event.EventShared.Models;
using OWP.Event.EventShared.Services;
using OWP.Models;
using OWP.Services;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.Event.UnitTests.EventShared.Services;

public class AdminSiteMatchServiceUnitTest
{
    private readonly EventRepository repository;
    private readonly GetAdminSiteInfoService getAdminSiteInfoService;
    private readonly AdminSiteMatchServices service;

    public AdminSiteMatchServiceUnitTest()
    {
        repository = Substitute.For<EventRepository>();
        getAdminSiteInfoService = Substitute.For<GetAdminSiteInfoService>();
        service = new AdminSiteMatchServicesImp(repository, getAdminSiteInfoService);
    }

    [Fact]
    public void UpdateMinorAdminMatchId_Always_ReturnExpect()
    {
        var matchInfo = new MatchInfo { OWPMatchId = 17, MinorBettypeId = 112, AdminSiteId = 1 };
        var minorAdminsiteIds = new List<MinorMatchInfo>
        {
            new() { AdminMatchId = 2, OwpMatchId = 17 }
        };
        repository.GetMinorAdminSiteIds(Arg.Any<IEnumerable<MinorMatch>>())
            .Returns(minorAdminsiteIds);

        service.UpdateMinorAdminMatchId(matchInfo, minorAdminsiteIds);

        Assert.Equal(2, matchInfo.AdminSiteId);
    }

    [Fact]
    public void UpdateMinorAdminMatchId_MatchInfoHasNotValue_ReturnMNoAction()
    {
        var matchInfo = new MatchInfo { OWPMatchId = 17 };
        var minorAdminsiteIds = new List<MinorMatchInfo>
        {
            new() { AdminMatchId = 2, OwpMatchId = 17 }
        };
        repository.GetMinorAdminSiteIds(Arg.Any<IEnumerable<MinorMatch>>())
            .Returns(minorAdminsiteIds);

        service.UpdateMinorAdminMatchId(matchInfo, minorAdminsiteIds);

        Assert.Null(matchInfo.AdminSiteId);
    }

    [Fact]
    public async Task GetMinorAdminSiteIds_ReturnsExpectedMinorMatchInfo()
    {
        // Arrange
        var matchInfos = new List<MatchInfo>
        {
            new () { MinorBettypeId = -3, AdminSiteId = 100, OWPMatchId = 200 },
            new () { MinorBettypeId = -100, AdminSiteId = 101, OWPMatchId = 201 },
            new () { MinorBettypeId = -4, AdminSiteId = 101, OWPMatchId = 201 },
            new () { AdminSiteId = 102, OWPMatchId = 202 }
        };

        // Act
        await service.GetMinorAdminSiteIds(matchInfos);

        // Assert
        await repository.Received().GetMinorAdminSiteIds(Arg.Is<IEnumerable<MinorMatch>>(
            x =>  x.First().SubType == 2000 && x.Last().SubType == 2101)
        );
    }

    [Fact]
    public async Task GetAdminSiteInfo_ReturnsExpectedAdminSiteMatchInfo()
    {
        const string parentMatchIds = "11,22";
        var adminSiteIdsWithOwpMatches = new List<AdminSiteIdModel>
        {
            new () { AdminSiteMatchId = 11, ParentMatchId = 33},
            new () { AdminSiteMatchId = 22 }
        };
        var adminSitesIdsOdds = new List<AdminSiteOddsModel>
        {
            new () { AdminSiteMatchId = 11, OddsStatus = "complete" },
            new () { AdminSiteMatchId = 22, OddsStatus = "running" }
        };

        getAdminSiteInfoService.GetAdminSiteMatchId(parentMatchIds).Returns(adminSiteIdsWithOwpMatches);
        getAdminSiteInfoService.GetAdminSiteInfo().Returns(adminSitesIdsOdds);

        var actual = await service.GetAdminSiteInfo(parentMatchIds);

        var expected = new List<AdminSiteMatchInfoModel>
        {
            new () { AdminSiteMatchId = 11, ParentMatchId = 33 }
        };

        DeepAssert.Equal(expected, actual);
    }
}

