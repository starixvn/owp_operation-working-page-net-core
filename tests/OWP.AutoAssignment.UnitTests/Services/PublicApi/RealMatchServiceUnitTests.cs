using NSubstitute;
using OWP.AutoAssignment.Models.PublicApi;
using OWP.AutoAssignment.Services.PublicApi;
using Xunit;

namespace OWP.AutoAssignment.UnitTests.Services.PublicApi;

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
    public async Task BuildRealMatchInfo_Always_ReturnListRealMatchInfo()
    {
        // Arrange
        repository.GetRealMatchCommonInfoByIds(Arg.Compat.Any<List<int>>())
            .Returns(MockListRealMatchCommon());
        repository.GetMaxMaxBetFTHDPInfo(Arg.Compat.Any<List<int>>())
            .Returns(MockListMaxBetHdpInfo());
        repository.GetMaxBetCorner(Arg.Compat.Any<List<MaxBetOfFTHDP>>())
            .Returns(MockListMaxBetConner());
        providerService.DetectProviderType(Arg.Compat.Any<string>())
            .ReturnsForAnyArgs(_ => [1]);
        extraTimeService.DetectExtraTime(Arg.Compat.Any<string>())
            .ReturnsForAnyArgs(_ => "ETPK");

        var expected = MockListRealMatchSender();
        var service = new RealMatchServiceImp(repository, extraTimeService, providerService);

        // Act
        var actual = await service
            .BuildRealMatchInfo(new List<MatchQueried> { new() { InfositeMatchId = 1, SabaType = null } });

        // Assert
        DeepAssert.Equal(expected, actual);
    }

    [Fact]
    public void BuildMaxBetMainMatch_Always_ReturnListMaxBetHDP()
    {
        var service = new RealMatchServiceImp(repository, extraTimeService, providerService);
        var maxBetOptions = MockListMaxBetHdpInfo();
        var realMatches = MockListRealMatchCommon();
        var expected = MockListMaxBetOfHdp();

        var actual = service.BuildMaxBetMainMatch(maxBetOptions, realMatches).ToList();

        DeepAssert.Equal(expected, actual);
    }

    public List<MaxBetCorner> MockListMaxBetConner()
        =>
        [
            new MaxBetCorner
            {
                InfoSiteMatchId = 1,
                MaxBetNextCorner = 20,
                MaxBetNoOfCorner = 20
            },

            new MaxBetCorner
            {
                InfoSiteMatchId = 3,
                MaxBetNextCorner = 30,
                MaxBetNoOfCorner = 30
            },

            new MaxBetCorner
            {
                InfoSiteMatchId = 4,
                MaxBetNextCorner = 40,
                MaxBetNoOfCorner = 40
            }
        ];

    public List<MaxBetFTHDPInfo> MockListMaxBetHdpInfo()
        =>
        [
            new MaxBetFTHDPInfo
            {
                MatchId = 1,
                MaxBet = 1000,
                HasTv = true,
                LeagueSettingId = 1
            },

            new MaxBetFTHDPInfo
            {
                MatchId = 1,
                MaxBet = 2000,
                HasTv = false,
                LeagueSettingId = 1
            },

            new MaxBetFTHDPInfo
            {
                MatchId = 3,
                MaxBet = 3000,
                HasTv = false,
                LeagueSettingId = 2
            },

            new MaxBetFTHDPInfo
            {
                MatchId = 4,
                MaxBet = 4000,
                HasTv = false,
                LeagueSettingId = 3
            },

            new MaxBetFTHDPInfo
            {
                MatchId = 4,
                MaxBet = 5000,
                HasTv = true,
                LeagueSettingId = 3
            }
        ];

    public List<RealMatchCommonInfo> MockListRealMatchCommon()
        =>
        [
            new RealMatchCommonInfo
            {
                InfoSiteId = 1,
                LeagueName = "Premier league",
                Provider = "SM",
                Remark = "ETPK",
                Tv = string.Empty,
                KOTime = new DateTime(2021, 11, 30),
                OtherProvider = "bg"
            },

            new RealMatchCommonInfo
            {
                InfoSiteId = 3,
                LeagueName = "Premier league",
                Provider = "SM",
                Remark = string.Empty,
                Tv = "HTV2",
                KOTime = new DateTime(2021, 11, 28),
                OtherProvider = "bg"
            },

            new RealMatchCommonInfo
            {
                InfoSiteId = 4,
                LeagueName = "Premier league",
                Remark = "PK",
                Provider = "SM",
                Tv = "K+",
                KOTime = new DateTime(2021, 11, 29),
                OtherProvider = ""
            }
        ];

    public List<MaxBetOfFTHDP> MockListMaxBetOfHdp()
        =>
        [
            new MaxBetOfFTHDP
            {
                InfoSiteMatchId = 1,
                MaxBetFTHDP = 2000,
                LeagueSettingId = 1
            },

            new MaxBetOfFTHDP
            {
                InfoSiteMatchId = 3,
                MaxBetFTHDP = 3000,
                LeagueSettingId = 2
            },

            new MaxBetOfFTHDP
            {
                InfoSiteMatchId = 4,
                MaxBetFTHDP = 5000,
                LeagueSettingId = 3
            }
        ];

    public List<RealMatchSender> MockListRealMatchSender()
        =>
        [
            new RealMatchSender
            {
                MatchId = 1,
                MaxBetMainMatch = 2000,
                MaxBetNextCorner = 20,
                MaxBetNoOfCorner = 20,
                ExtraTime = "ETPK",
                Provider = [1],
                KOTime = "2021-11-30T00:00:00",
                HasTV = false,
                LeagueSettingId = 1
            },

            new RealMatchSender
            {
                MatchId = 3,
                MaxBetMainMatch = 3000,
                MaxBetNextCorner = 30,
                MaxBetNoOfCorner = 30,
                ExtraTime = "ETPK",
                Provider = [1],
                KOTime = "2021-11-28T00:00:00",
                HasTV = true,
                LeagueSettingId = 2
            },

            new RealMatchSender
            {
                MatchId = 4,
                MaxBetMainMatch = 5000,
                MaxBetNextCorner = 40,
                MaxBetNoOfCorner = 40,
                ExtraTime = "ETPK",
                Provider = [1],
                KOTime = "2021-11-29T00:00:00",
                HasTV = true,
                LeagueSettingId = 3
            }
        ];
}