using NSubstitute;
using OWP.Event.EventShared;
using OWP.Event.EventShared.Models;
using OWP.Event.EventShared.Services;
using Xunit;

namespace OWP.Event.UnitTests.EventShared.Services;

public class AdminSiteMatchServiceUnitTest
{
    private readonly EventRepository repository;
    private readonly AdminSiteMatchServices service;

    public AdminSiteMatchServiceUnitTest()
    {
        repository = Substitute.For<EventRepository>();
        service = new AdminSiteMatchServicesImp(repository);
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
            x => x.First().SubType == 2000)
        );

        await repository.Received().GetMinorAdminSiteIds(Arg.Is<IEnumerable<MinorMatch>>(
            x => x.Last().SubType == 2101)
        );
    }

}

