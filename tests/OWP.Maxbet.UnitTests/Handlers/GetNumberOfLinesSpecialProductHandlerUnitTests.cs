using Fanex.Data.Repository;
using NSubstitute;
using OWP.Maxbet.Criteria;
using OWP.Maxbet.Handlers;
using OWP.Maxbet.Models;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.Maxbet.UnitTests.Handlers
{
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

            NumberOfLines actual = await handler.Handle(new GetNumberOfLinesSpecialProductHandler.Request { OWPMatchId = 3 }, new CancellationToken(false));

            DeepAssert.Equals(expected, actual);
        }
    }
}
