namespace OWP.Maxbet.UnitTests.Handlers;

using Criteria;
using Fanex.Data.Repository;
using Maxbet.Handlers;
using Models;
using NSubstitute;
using Xunit;

public class GetNumberOfLinesSpecialProductHandlerUnitTests
{
    private IDynamicRepository dynamicRepository;
    private GetNumberOfLinesSpecialProductHandler.Handler handler;
    private void Setup()
    {
        dynamicRepository = Substitute.For<IDynamicRepository>();
        handler = new GetNumberOfLinesSpecialProductHandler.Handler(dynamicRepository);
    }

    [Fact]
    public async Task Handler_MatchWithTwoPlusFourLinesNoOfHDP_ReturnData()
    {
        Setup();
        IEnumerable<MatchResource> noOfHdp = new List<MatchResource> { new() { ResourceId = 10 } };
        dynamicRepository.FetchAsync<MatchResource>(Arg.Compat.Any<MatchNoOfHdpGetCriteria>()).Returns(Task.FromResult(noOfHdp));
        var expected = new NumberOfLines { CreditFt = 1, CreditFirstHalf = 1, LicenseeFt = 0, LicenseeFirstHalf = 0 };

        var actual = await handler.Handle(new GetNumberOfLinesSpecialProductHandler.Request { OWPMatchId = 3 }, new CancellationToken(false));

        Equals(expected, actual);
    }
}