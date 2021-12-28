namespace OWP.AutoAssignment.UnitTests.Services.PublicApi
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoAssignment.Services.PublicApi;
    using Models.PublicApi;
    using NSubstitute;
    using Xunit;
    using Xunit.Asserts.Compare;

    public class SabaMatchServiceUnitTests
    {
        private readonly Repository repository;

        public SabaMatchServiceUnitTests()
        {
            repository = Substitute.For<Repository>();
        }

        [Fact]
        public async Task BuildSabaMatch_Always_ReturnListSaBaMatch()
        {
            var matchsQueried = MockListMatchsQueried();
            repository.GetSabaMatchCommonInfoByIds(Arg.Compat.Any<List<int>>())
                .Returns(MockListSabaMatchInfo());
            var expected = GetExpected();
            var service = new SabaMatchServiceImp(repository);

            var actual = await service.BuildSabaMatchInfo(matchsQueried);

            DeepAssert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetSabaMaxbet_Always_ReturnValues()
        {
            var expected = new List<MaxBetOfFTHDP>()
                {
                    new MaxBetOfFTHDP { InfoSiteMatchId = 1, MaxBetFTHDP = 1 }
                };

            repository.GetMaxMaxbetFTHDP(Arg.Compat.Any<int[]>())
                .Returns(expected);
            var service = new SabaMatchServiceImp(repository);

            var act = await service.GetSabaMaxbet(new int[] { 1 });

            DeepAssert.Equal(expected, act);
        }

        public List<SabaMatchCommonInfo> MockListSabaMatchInfo()
            => new List<SabaMatchCommonInfo>()
            {
                new()
                {
                    InfositeId = 1,
                    Location = "Q.12",
                    KOTime = new DateTime(2021, 11, 30)
                },
                new()
                {
                    InfositeId = 2,
                    Location = "Q.11",
                    KOTime = new DateTime(2021, 11, 29)
                },
                new()
                {
                    InfositeId = 3,
                    Location = "Q.10",
                    KOTime = new DateTime(2021, 11, 28)
                },
            };

        public List<MatchQueried> MockListMatchsQueried()
            => new()
            {
                new()
                {
                    InfositeMatchId = 1,
                    SabaType = 1
                },
                new()
                {
                    InfositeMatchId = 2,
                    SabaType = 2
                },
                new()
                {
                    InfositeMatchId = 3,
                    SabaType = 3
                },
            };

        public List<SabaMatchSender> GetExpected()
            => new()
            {
                new()
                {
                    MatchId = 1,
                    SabaType = 1,
                    KOTime = "2021-11-30T00:00:00"
                },
                new()
                {
                    MatchId = 2,
                    SabaType = 2,
                    KOTime = "2021-11-29T00:00:00"
                },
                new()
                {
                    MatchId = 3,
                    SabaType = 3,
                    KOTime = "2021-11-28T00:00:00"
                }
            };
    }
}