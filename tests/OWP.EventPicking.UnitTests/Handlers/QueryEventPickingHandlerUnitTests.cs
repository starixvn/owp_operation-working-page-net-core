using NSubstitute;
using OWP.EventPicking.Handlers;
using OWP.EventPicking.Models;
using OWP.EventPicking.Services;
using Xunit;

namespace OWP.EventPicking.UnitTests.Handlers;

using Xunit.Asserts.Compare;

public class QueryEventPickingHandlerUnitTests
{
    private readonly QueryEventPickingServiceImp queryEventPickingService;
    private readonly QueryEventPickingHandler service;

    private readonly BuildMainAndChildMatches buildMainAndChildMatches;

    public QueryEventPickingHandlerUnitTests()
    {
        queryEventPickingService = Substitute.For<QueryEventPickingServiceImp>();
        buildMainAndChildMatches = Substitute.For<BuildMainAndChildMatches>();
        service = new QueryEventPickingHandler(queryEventPickingService);
    }

    [Fact]
    public async void Handle()
    {
        var request = new QueryEventPickingRequest
        {
            Date = new DateTime(2022, 11, 07),
            League = new List<int> { 1, 2 },
            SelectGame = new List<int> { 1 },
            FilterMatch = new List<string> { "Red" },
            HasOtherProvider = new List<string> { "yes", "no" },
            HasProvider = new List<string> { "yes", "no" },
            FilterMatchCategory = new List<int> { 0, 1 },
            UpdateType = new List<int?> { null }
        };
        var matches = new List<EventPickingModel>
        {
            new(){ InfoSiteMatchId = 111, RAndDInfos =new List<RAndDInfo>(), OWPLeagueId = 1, IsNoOffer = true, RowColor = "Red" },
            new(){ InfoSiteMatchId = 222, RAndDInfos =new List<RAndDInfo>(), OWPLeagueId = 2, IsNoOffer = true, RowColor = "Red" },
            new(){ InfoSiteMatchId = 333, RAndDInfos =new List<RAndDInfo>(), LeagueName = "TESTING", RowColor = ""},
            new(){ InfoSiteMatchId = 444, RAndDInfos =new List<RAndDInfo>(), IsNoOffer = false, RowColor = "" },
            new(){ InfoSiteMatchId = 555, RAndDInfos =new List<RAndDInfo>(), IsNoOffer = true, RowColor = "Gray" },
            new(){ InfoSiteMatchId = 666, RAndDInfos =new List<RAndDInfo>(), HomeName = "(ET)", RowColor = ""  },
            new(){ InfoSiteMatchId = 777, RAndDInfos =new List<RAndDInfo>(), HomeName = "(PEN)", RowColor = "" }
        };

        buildMainAndChildMatches.GetMainEvent(new DateTime(2022, 11, 07)).Returns(matches);

        var expected = new List<EventPickingModel>
        {
            new(){ InfoSiteMatchId = 111, RAndDInfos =new List<RAndDInfo>(), OWPLeagueId = 1, IsNoOffer = true, RowColor = "Red" },
            new(){ InfoSiteMatchId = 222, RAndDInfos =new List<RAndDInfo>(), OWPLeagueId = 2, IsNoOffer = true, RowColor = "Red" },
        };

        var actual = await service.Handle(request, CancellationToken.None);

        DeepAssert.Equal(expected, actual);
    }
}