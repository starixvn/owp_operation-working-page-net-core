using NSubstitute;
using OWP.LiveGameList.Models;
using OWP.LiveGameList.Services;
using OWP.Services;
using Xunit;

namespace OWP.LiveGameList.UnitTests.Services;

public class AdminSiteMatchServiceUnitTest
{
    private readonly Repository repository;
    private readonly GetAdminSiteInfoService getAdminSiteInfoService;

    public AdminSiteMatchServiceUnitTest()
    {
        repository = Substitute.For<Repository>();
        getAdminSiteInfoService = Substitute.For<GetAdminSiteInfoService>();
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

        var service = new AdminSiteMatchServicesImp(repository, getAdminSiteInfoService);
        service.UpdateMinorAdminMatchId(matchInfo, minorAdminsiteIds);

        Assert.Equal(2, matchInfo.AdminSiteId);
    }
}

