namespace OWP.AutoAssignment.UnitTests.Services.PublicApi
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoAssignment.Services.PublicApi;
    using Models.PublicApi;
    using NSubstitute;
    using Xunit;
    using Xunit.Asserts.Compare;

    public class RealMatchServiceUnitTests
    {
        private readonly Repository repository;
        private readonly ExtraTimeDetectorService extraTimeService;
        private readonly ProviderDetectorService providerService;
        public RealMatchServiceUnitTests()
        {
            repository = Substitute.For<Repository>();
            extraTimeService = Substitute.For<ExtraTimeDetectorService>();
            providerService = Substitute.For<ProviderDetectorService>();
        }

        [Fact]
        public async Task BuildMaxBetMainMatch_Always_ReturnListMaxBetHDP()
        {
            var service = new RealMatchServiceImp(repository, extraTimeService, providerService);
            var maxBetOptions = MockListMaxBetFThdpInfo();
            var realMatches = MockListRealMatchCommon();
            var expected = MockListMaxBetOfFthdp();

            var actual = service.BuildMaxBetMainMatch(maxBetOptions, realMatches).ToList();

            DeepAssert.Equal(expected, actual);
        }

        public List<MaxBetFTHDPInfo> MockListMaxBetFThdpInfo()
            => new()
            {
                new()
                {
                    MatchId = 1,
                    MaxBet = 1000,
                    HasTv = true,
                },
                new()
                {
                    MatchId = 1,
                    MaxBet = 2000,
                    HasTv = false,
                },
                new()
                {
                    MatchId = 3,
                    MaxBet = 3000,
                    HasTv = false,
                },
                new()
                {
                    MatchId = 4,
                    MaxBet = 4000,
                    HasTv = false,
                },
                new()
                {
                    MatchId = 4,
                    MaxBet = 5000,
                    HasTv = true,
                },
            };

        public List<RealMatchCommonInfo> MockListRealMatchCommon()
            => new()
            {
                new()
                {
                    InfoSiteId = 1,
                    Tv = string.Empty

                },
                new()
                {
                    InfoSiteId = 3,
                    Tv = "HTV2"
                },
                new()
                {
                    InfoSiteId = 4,
                    Tv = "K+"
                },
            };

        public List<MaxBetOfFTHDP> MockListMaxBetOfFthdp()
            => new()
            {
                new()
                {
                    InfoSiteMatchId = 1,
                    MaxBetFTHDP = 2000,

                },
                new()
                {
                    InfoSiteMatchId = 3,
                    MaxBetFTHDP = 3000,
                },
                new()
                {
                    InfoSiteMatchId = 4,
                    MaxBetFTHDP = 5000,
                },
            };
    }
}
