namespace OWP.EventPicking.UnitTests.Services;

using EventPicking.Services;
using MaxbetSetting.Models;
using Models;
using Xunit;

public class BuildMatchOfferByLiveSettingUnitTests
{
    private readonly BuildMatchOfferByLiveSettingImp service;
    public BuildMatchOfferByLiveSettingUnitTests()
    {
        service = new BuildMatchOfferByLiveSettingImp();
    }

    [Fact]
    public void BuildMatchOfferByLiveSetting()
    {
        var maxbetSettings = new Dictionary<decimal, MaxBetSettingModel>
        {
            { 5000, new MaxBetSettingModel {
                FT_HDP = 5000,
                NoOfCorner_FT_HDP = 2000,
                NoOfCorner_FT_OU = 100,
                NextCorner_FT_HDP = 12000,
                SecondAccept_Fantasy = "fantasy offer setting",
                SecondAccept_Corner = "offer corner"
            } },
            { 4000, new() {
                FT_HDP = 4000,
                NextCorner_FT_HDP = 400,
                NextBooking_FT_HDP = 100,
                SecondAccept_Main="offer main",
                SecondAccept_TotalBooking = "offer booking"
            } },
            { 3000, new() { FT_HDP = 3000, TotalBooking_FT_HDP = 300, TotalBooking_FT_OU = 100} },
            { -1000, new() { FT_HDP = -1000, NextCorner_FT_HDP = 200, NextBooking_FT_HDP = 333 }}
        };
        var mockMainMatches = MockMainMatches();
        var mockChildMatches = MockChildMatches();
        service.Build(mockMainMatches, mockChildMatches, maxbetSettings);

        Assert.Equal("fantasy offer", mockMainMatches[0].MatchOffer);
        Assert.Equal("fantasy offer setting", mockMainMatches[1].MatchOffer);
        Assert.Equal("offer main", mockMainMatches[2].MatchOffer);
        Assert.Equal("offer main match", mockMainMatches[3].MatchOffer);
        Assert.Null(mockMainMatches[4].MatchOffer);

        Assert.Equal("offer corner", mockChildMatches[0].MatchOffer);
        Assert.Equal("offer corner", mockChildMatches[1].MatchOffer);
        Assert.Equal("offer booking", mockChildMatches[2].MatchOffer);
        Assert.Null(mockChildMatches[3].MatchOffer);
        Assert.Null(mockChildMatches[4].MatchOffer);
    }

    public List<EventPickingModel> MockMainMatches()
        => new()
        {
            new EventPickingModel
            {
                OWPLeagueId = 111,
                InfoSiteMatchId = 1111,
                LeagueName = "Fantasy match",
                MaxBetHDPNumber = 5000,
                MatchOffer = "fantasy offer"
            },
            new EventPickingModel
            {
                OWPLeagueId = 222,
                InfoSiteMatchId = 2222,
                LeagueName = "Fantasy match B",
                MaxBetHDPNumber = 5000,
                MatchOffer = null
            },
            new EventPickingModel
            {
                OWPLeagueId = 333,
                InfoSiteMatchId = 3333,
                LeagueName = "League A",
                MaxBetHDPNumber = 4000,
                MatchOffer = null
            },
            new EventPickingModel
            {
                OWPLeagueId = 444,
                InfoSiteMatchId = 4444,
                LeagueName = "League A",
                MaxBetHDPNumber = 4000,
                MatchOffer = "offer main match"
            },
            new EventPickingModel
            {
                OWPLeagueId = 5,
                InfoSiteMatchId = 5555,
                LeagueName = "League A",
                MaxBetHDPNumber = null,
                MatchOffer = null
            }
        };
    public List<EventPickingModel> MockChildMatches()
        => new()
        {
            new EventPickingModel
            {
                OWPLeagueId = 100,
                InfoSiteMatchId = 1111,
                MinorBettypeId = -3
            },
            new EventPickingModel
            {
                OWPLeagueId = 200,
                InfoSiteMatchId = 2222,
                MinorBettypeId = -4
            },
            new EventPickingModel
            {
                OWPLeagueId = 300,
                InfoSiteMatchId = 3333,
                MinorBettypeId = -6
            },
            new EventPickingModel
            {
                OWPLeagueId = 500,
                InfoSiteMatchId = 5555,
                MinorBettypeId = -6
            },
            new EventPickingModel
            {
                OWPLeagueId = 400,
                InfoSiteMatchId = 3333,
                MinorBettypeId = -99
            }
        };
}