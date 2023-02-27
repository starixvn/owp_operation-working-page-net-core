using Fanex.NAP.Client.Helpers.Web;
using NSubstitute;
using OWP.LiveGameList.Criteria;
using OWP.LiveGameList.Models;
using OWP.LiveGameList.Services;
using OWP.Services;
using Xunit;
using GetAdminSiteMatchIdService = OWP.LiveGameList.Services.GetAdminSiteMatchIdService;

namespace OWP.LiveGameList.UnitTests.Services;

public class EventInformationServiceUnitTest
{
    private readonly INapSession currentSession;
    private readonly GetAdminSiteMatchIdService getAdminSiteMatchIdService;
    private readonly PermissionService permissionService;
    private readonly Repository repository;

    public EventInformationServiceUnitTest()
    {
        currentSession = Substitute.For<INapSession>();
        getAdminSiteMatchIdService = Substitute.For<GetAdminSiteMatchIdService>();
        permissionService = Substitute.For<PermissionService>();
        repository = Substitute.For<Repository>();
    }

    [Fact]
    public async Task GetEvents_Always_ReturnExpect()
    {
        repository.GetEventInformation(Arg.Any<GetEventsCriteria>())
            .Returns((MockMatchInfo(), MockAssignment(), MockMatchSetting(), MockTelevision(), MockMatchOffers(),
                MockTicketInfo()));

        var expected = new List<MatchInfo>
        {
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
            LengthLeague = 10
            }
        };
        var service = new EventInformationServiceImp(repository, currentSession, getAdminSiteMatchIdService, permissionService);
        var actual = await service.GetEvents(new DateTime(), "1,2,3,4");

        Assert.Equal(expected.First().IPTVCopy, actual.First().IPTVCopy);
        Assert.Equal(expected.First().LengthLeague, actual.First().LengthLeague);
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

    public IEnumerable<TicketInfo> MockTicketInfo()
        => new List<TicketInfo> { new() { OwpMatchId = 1 } };

}
