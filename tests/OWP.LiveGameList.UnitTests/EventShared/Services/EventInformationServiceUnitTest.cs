using Fanex.Data.Repository;
using NSubstitute;
using OWP.Event.EventShared;
using OWP.Event.EventShared.Models;
using OWP.Event.EventShared.Services;
using Xunit;

namespace OWP.Event.UnitTests.EventShared.Services;

public class EventInformationServiceUnitTest
{
    private readonly AdminSiteMatchServices adminSiteMatchServices;
    private readonly EventRepository repository;
    private readonly InfoSiteTimeServices infoSiteTimeService;

    [Fact]
    public void GetEvents_Always_ReturnExpect()
    {
        var eventInfo = new EventInformation
        {
            MatchInfo = MockMatchInfo(),
            AssignmentsPair = MockAssignment(),
            MinorAssignmentsPair = new Dictionary<int, List<Assignment>>(),
            MatchSettingsPair = MockMatchSetting(),
            MinorMatchSettingsPair = new Dictionary<int, List<MatchSetting>>(),
            HasTicketModel = new List<HasTicketModel>(),
            TelevisionsPair = MockTelevision(),
            MatchOffersPair = MockMatchOffers()
        };
        repository.GetEventInformation(Arg.Any<CriteriaBase>()).Returns(eventInfo);

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
            Phone = "ok none "
            }
        };
        var service = new EventInformationServiceImp(adminSiteMatchServices, infoSiteTimeService);
        var actual = service.BuildEvent(eventInfo, eventInfo.MatchInfo.First());

        Assert.Equal(expected.First().IPTVCopy, actual.IPTVCopy);
    }

    public List<MatchInfo> MockMatchInfo()
        => new()
        {
            new MatchInfo
            {
                OWPMatchId = 1,
                League = "V-league",
                Remark = "ok",
                Round = "final",
                Division = "job",
                Provider = "provider",
                WatchBet = "ok",
                Phone = "ok none "
            }
        };

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
}
