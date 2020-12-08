namespace OWP.API.SyncMaxbet.UnitTests.Handlers
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Fanex.Data.Repository;
    using NSubstitute;
    using OWP.API.SyncMaxbet.Criterias;
    using OWP.API.SyncMaxbet.Handlers;
    using OWP.API.SyncMaxbet.Models;
    using OWP.Enums;
    using Xunit;
    using Xunit.Asserts.Compare;

    public class GetNumberOfLinesHandlerUnitTests
    {
        private IDynamicRepository dynamicRepository;
        private GetNumberOfLinesHandler.Handler handler;

        [Fact]
        public async Task Handler_MatchWithTwoPlusFourLinesNoOfHDP_ReturnData()
        {
            Setup();
            IEnumerable<MatchResource> noOfHdp = new List<MatchResource> { new MatchResource { ResourceId = (int)EnumNoOfHdp.TwoPlusFourLines } };
            dynamicRepository.FetchAsync<MatchResource>(Arg.Compat.Any<MatchNoOfHdpGetCriteria>()).Returns(Task.FromResult(noOfHdp));
            var expected = new NumberOfLines { CreditFt = 2, CreditFirstHaft = 2, LicenseeFt = 4, LicenseeFirstHaft = 3 };

            NumberOfLines actual = await handler.Handle(new GetNumberOfLinesHandler.Request { OWPMatchId = 123 }, new CancellationToken(false));

            DeepAssert.Equals(expected, actual);
        }

        [Fact]
        public async Task Handler_MatchWithThreePlusThreeLinesNoOfHDP_ReturnData()
        {
            Setup();
            IEnumerable<MatchResource> noOfHdp = new List<MatchResource> { new MatchResource { ResourceId = (int)EnumNoOfHdp.ThreePlusThreeLines } };
            dynamicRepository.FetchAsync<MatchResource>(Arg.Compat.Any<MatchNoOfHdpGetCriteria>()).Returns(Task.FromResult(noOfHdp));
            var expected = new NumberOfLines { CreditFt = 3, CreditFirstHaft = 2, LicenseeFt = 3, LicenseeFirstHaft = 2 };

            NumberOfLines actual = await handler.Handle(new GetNumberOfLinesHandler.Request { OWPMatchId = 123 }, new CancellationToken(false));

            DeepAssert.Equals(expected, actual);
        }

        [Fact]
        public async Task Handler_MatchWithThreePlusFourLinesNoOfHDP_ReturnData()
        {
            Setup();
            IEnumerable<MatchResource> noOfHdp = new List<MatchResource> { new MatchResource { ResourceId = (int)EnumNoOfHdp.ThreePlusFourLines } };
            dynamicRepository.FetchAsync<MatchResource>(Arg.Compat.Any<MatchNoOfHdpGetCriteria>()).Returns(Task.FromResult(noOfHdp));
            var expected = new NumberOfLines { CreditFt = 3, CreditFirstHaft = 2, LicenseeFt = 4, LicenseeFirstHaft = 3 };

            NumberOfLines actual = await handler.Handle(new GetNumberOfLinesHandler.Request { OWPMatchId = 123 }, new CancellationToken(false));

            DeepAssert.Equals(expected, actual);
        }

        [Fact]
        public async Task Handler_MatchWithFourPlusFourLinesNoOfHDP_ReturnData()
        {
            Setup();
            IEnumerable<MatchResource> noOfHdp = new List<MatchResource> { new MatchResource { ResourceId = (int)EnumNoOfHdp.FourPlusFourLines } };
            dynamicRepository.FetchAsync<MatchResource>(Arg.Compat.Any<MatchNoOfHdpGetCriteria>()).Returns(Task.FromResult(noOfHdp));
            var expected = new NumberOfLines { CreditFt = 4, CreditFirstHaft = 3, LicenseeFt = 4, LicenseeFirstHaft = 3 };

            NumberOfLines actual = await handler.Handle(new GetNumberOfLinesHandler.Request { OWPMatchId = 123 }, new CancellationToken(false));

            DeepAssert.Equals(expected, actual);
        }

        [Fact]
        public async Task Handler_MatchWithNoNoOfHDP_ReturnData()
        {
            Setup();
            IEnumerable<MatchResource> noOfHdp = new List<MatchResource> { new MatchResource { ResourceId = (int)EnumNoOfHdp.Empty } };
            dynamicRepository.FetchAsync<MatchResource>(Arg.Compat.Any<MatchNoOfHdpGetCriteria>()).Returns(Task.FromResult(noOfHdp));
            var expected = new NumberOfLines { CreditFt = 2, CreditFirstHaft = 2, LicenseeFt = 3, LicenseeFirstHaft = 2 };

            NumberOfLines actual = await handler.Handle(new GetNumberOfLinesHandler.Request { OWPMatchId = 123 }, new CancellationToken(false));

            DeepAssert.Equals(expected, actual);
        }

        [Fact]
        public async Task Handler_MatchWithFirstHDPNoOfHDP_ReturnData()
        {
            Setup();
            IEnumerable<MatchResource> noOfHdp = new List<MatchResource> { new MatchResource { ResourceId = (int)EnumNoOfHdp.FirstHDP } };
            dynamicRepository.FetchAsync<MatchResource>(Arg.Compat.Any<MatchNoOfHdpGetCriteria>()).Returns(Task.FromResult(noOfHdp));
            var expected = new NumberOfLines();

            NumberOfLines actual = await handler.Handle(new GetNumberOfLinesHandler.Request { OWPMatchId = 123 }, new CancellationToken(false));

            DeepAssert.Equals(expected, actual);
        }

        private void Setup()
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();
            handler = new GetNumberOfLinesHandler.Handler(dynamicRepository);
        }
    }
}