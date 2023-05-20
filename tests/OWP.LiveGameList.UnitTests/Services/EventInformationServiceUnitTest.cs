using Fanex.NAP.Client.Helpers.Web;
using NSubstitute;
using OWP.LiveGameList.Models;
using OWP.LiveGameList.Services;
using OWP.Services;
using Xunit;

namespace OWP.LiveGameList.UnitTests.Services;

public class EventInformationServiceUnitTest
{
    private readonly INapSession currentSession;
    private readonly AdminSiteMatchServices adminSiteMatchServices;
    private readonly PermissionService permissionService;
    private readonly Repository repository;
    private readonly InfoSiteTimeServices infoSiteTimeService;

    public EventInformationServiceUnitTest()
    {
        currentSession = Substitute.For<INapSession>();
        adminSiteMatchServices = Substitute.For<AdminSiteMatchServices>();
        infoSiteTimeService = Substitute.For<InfoSiteTimeServices>();
        permissionService = Substitute.For<PermissionService>();
        repository = Substitute.For<Repository>();
    }

    [Fact]
    public async Task GetEvents_Always_ReturnExpect()
    {
        var eventInfo = new EventInformation
        {
            MatchInfos = MockMatchInfo(),
            AssignmentsDict = MockAssignment(),
            MinorAssignmentsDict = new Dictionary<int, List<Assignment>>(),
            MatchSettingsDict = MockMatchSetting(),
            MinorMatchSettingsDict = new Dictionary<int, List<MatchSetting>>(),
            TelevisionsDict = MockTelevision(),
            MatchOffersDict = MockMatchOffers(),
            TicketInfosDict = MockTicketInfo()
        };
        repository.GetEventInformation(Arg.Any<DateTime>(), Arg.Any<string>(), Arg.Any<int>(), Arg.Any<bool>()).Returns(eventInfo);

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
            LengthLeague = 10
            }
        };
        var service = new EventInformationServiceImp(repository, currentSession, adminSiteMatchServices, permissionService, infoSiteTimeService);
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

    public Dictionary<int, List<Assignment>> MockAssignment()
    {
        return new Dictionary<int, List<Assignment>>
        {
            {
                117, new List<Assignment>
                {
                    new() { BettypeId = 1, MinorBettypeId = 1, AssignmentType = 1, OWPMatchId = 117 }
                }
            }
        };
    }

    public Dictionary<int, List<MatchSetting>> MockMatchSetting()
        => new()
        {
            {
                1,
                new List<MatchSetting> { new() { OWPMatchId = 1 } }
            }
        };

    public Dictionary<int, List<Television>> MockTelevision()
    {
        return new Dictionary<int, List<Television>>
        {
            {
                1, new List<Television> { new() { TVCode = "ok", OWPMatchId = 1 } }
            }
        };
    }

    public Dictionary<int, List<MatchOffer>> MockMatchOffers()
    {
        return new Dictionary<int, List<MatchOffer>>
        {
            {
                1, new List<MatchOffer> { new() { MatchOfferValue = "6s", OWPMatchId = 1 } }
            }
        };
    }

    public Dictionary<int, List<TicketInfo>> MockTicketInfo()
    {
        return new Dictionary<int, List<TicketInfo>>
        {
            {
                1, new List<TicketInfo> { new() { OwpMatchId = 1 } }
            }
        };
    }
}
