using NSubstitute;
using OWP.EventPicking.Handlers;
using OWP.EventPicking.Models;
using OWP.EventPicking.Services;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.EventPicking.UnitTests.Handlers;

public class QueryEventPickingHandlerUnitTests
{
    private readonly BuildMainAndChildMatches buildMainAndChildMatches;
    private readonly QueryEventPickingHandler service;

    public QueryEventPickingHandlerUnitTests()
    {
        buildMainAndChildMatches = Substitute.For<BuildMainAndChildMatches>();

        service = new QueryEventPickingHandler(buildMainAndChildMatches);
    }

    [Fact]
    public async void Handle()
    {
        var request = new QueryEventPickingRequest
        {
            Date = new DateTime(2022, 11, 07),
            SelectGame = new List<int> { 1 },
            FilterMatch = new List<string> { "Red" },
            HasOtherProvider = new List<string> { "yes", "no" },
            HasProvider = new List<string> { "yes", "no" },
            FilterMatchCategory = new List<int> { 0, 1 }
        };
        var matches = new List<EventPickingModel>
        {
            new(){ InfoSiteMatchId = 111, IsNoOffer = true, RowColor = "Red" },
            new(){ InfoSiteMatchId = 222, IsNoOffer = true, RowColor = "Red" },
            new(){ InfoSiteMatchId = 333, LeagueName = "TESTING", RowColor = ""},
            new(){ InfoSiteMatchId = 444, IsNoOffer = false, RowColor = "" },
            new(){ InfoSiteMatchId = 555, IsNoOffer = true, RowColor = "Gray" },
            new(){ InfoSiteMatchId = 666, HomeName = "(ET)", RowColor = ""  },
            new(){ InfoSiteMatchId = 777, HomeName = "(PEN)", RowColor = "" }
        };

        buildMainAndChildMatches.GetMainAndChildMatchEvent(new DateTime(2022, 11, 07)).Returns(matches);

        var expected = new List<EventPickingModel>
        {
            new(){ InfoSiteMatchId = 111, IsNoOffer = true, RowColor = "Red" },
            new(){ InfoSiteMatchId = 222, IsNoOffer = true, RowColor = "Red" }
        };

        var actual = await service.Handle(request, CancellationToken.None);

        DeepAssert.Equal(expected, actual);
    }
}