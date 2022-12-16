using Fanex.NAP.Client.Helpers.Web;
using NSubstitute;
using OWP.TaskList.Criteria;
using OWP.TaskList.Models;
using OWP.TaskList.Services;
using OWP.Services;
using Xunit;
using GetAdminSiteMatchIdService = OWP.TaskList.Services.GetAdminSiteMatchIdService;

namespace OWP.TaskList.UnitTests.Services;


public class GetEventByDayServiceUnitTests
{
    private readonly Repository repository;

    private readonly INapSession session;

    private readonly PermissionService permissionService;

    private readonly GetAdminSiteMatchIdService getAdminSiteMatchIdService;
    public GetEventByDayServiceUnitTests()
    {
        session = Substitute.For<INapSession>();
        getAdminSiteMatchIdService = Substitute.For<GetAdminSiteMatchIdService>();
        permissionService = Substitute.For<PermissionService>();
        repository = Substitute.For<Repository>();
    }

    [Fact]
    public async Task GetEvents_Always_ReturnExpect()
    {
        repository.GetTaskList(Arg.Any<GetTaskListCriteria>())
            .Returns((MockMatchInfo(), MockAssignment(), MockMatchSetting(), MockTelevision(), MockMatchOffers(), MockMainInfos(),
                MockTicketInfo()));

        var expected = new List<MatchInfo>() {
            new () {
            OWPMatchId = 1,
            IPTVCopy = "ok/ok",
            League = "V-league",
            Remark = "ok",
            Round = "final",
            Division = "job",
            Provider = "provider",
            WatchBet = "ok",
            Phone = "ok none " ,
            HasTicket = true,
            }
        };
        var service = new GetEventByDayServiceImp(repository, session, permissionService, getAdminSiteMatchIdService);
        var actual = await service.GetEventByDay(new DateTime(), true);

        Assert.Equal(expected.First().League, actual.Matches.First().League);
    }

    public IEnumerable<MatchInfo> MockMatchInfo()
        => new List<MatchInfo> { new () {
            OWPMatchId = 1,
            League = "V-league",
            Remark = "ok",
            Round = "final",
            Division = "job",
            Provider = "provider",
            WatchBet = "ok",
            Phone = "ok none " } };

    public IEnumerable<Assignment> MockAssignment()
        => new List<Assignment> { new() { BettypeId = 1, MinorBettypeId = 1, AssignmentType = 1, OWPMatchId = 117 } };

    public IEnumerable<MatchSetting> MockMatchSetting()
        => new List<MatchSetting> { new() { OWPMatchId = 1 } };

    public IEnumerable<Television> MockTelevision()
        => new List<Television> { new() { TVCode = "ok", OWPMatchId = 1 } };

    public IEnumerable<MatchOffer> MockMatchOffers()
        => new List<MatchOffer> { new() { MatchOfferValue = "6s", OWPMatchId = 1 } };

    public IEnumerable<MainMatchInfo> MockMainInfos()
        => new List<MainMatchInfo> { new() { OWPMatchId = 1 , IsCompletedMatch = false} };

    public IEnumerable<TicketInfo> MockTicketInfo()
        => new List<TicketInfo> { new() { OwpMatchId = 1 } };

}
