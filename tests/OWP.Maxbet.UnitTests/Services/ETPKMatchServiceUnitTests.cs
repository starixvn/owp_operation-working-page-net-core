using NSubstitute;
using OWP.Api.Maxbet;
using OWP.Api.Maxbet.Models;
using OWP.Api.Maxbet.Services;
using Xunit;

namespace OWP.Maxbet.UnitTests.Services;

using Xunit.Asserts.Compare;

public class ETPKMatchServiceUnitTests
{
    private readonly ETPKMatchService etpkMatchService;
    private readonly FutsalMatchService futsalMatchService;
    private readonly MainMatchService mainMatchService;
    private readonly SabaMatchService sabaMatchService;
    private readonly SpecialProductService specialProductService;
    private readonly MaxbetRepository maxbetRepository;

    public ETPKMatchServiceUnitTests()
    {
        futsalMatchService = Substitute.For<FutsalMatchService>();
        mainMatchService = Substitute.For<MainMatchService>();
        sabaMatchService = Substitute.For<SabaMatchService>();
        specialProductService = Substitute.For<SpecialProductService>();
        maxbetRepository = Substitute.For<MaxbetRepository>();
        etpkMatchService = new ETPKMatchServiceImp(futsalMatchService, mainMatchService, sabaMatchService, specialProductService, maxbetRepository);
    }


    [InlineData("SABA FIFA 16 Mins")]
    [InlineData("SABA FIFA 6 Mins")]
    [InlineData("M88 SABA FIFA 16 Mins")]
    [InlineData("SABA PES 15 Mins")]
    [InlineData("PES 20 Mins")]
    [InlineData("SABA PES 5 Mins")]
    [Theory]
    public async Task GetMaxBetETPK_Always_ReturnSaBaMaxBetInfo(string leagueName)
    {
        maxbetRepository.GetMatchInfoByAdminSiteId(Arg.Any<int>()).Returns(new MatchInfo
        {
            owpLeagueName = leagueName,
            owpMatchId = 3
        });
        var expected = new MaxbetInfo { AdminsiteID = 1, Maxbet = new MaxbetSpread { FirstHalfHandicap = 1000 } };
        sabaMatchService.GetMaxbetInfoSaba(Arg.Any<int>(), Arg.Any<int>()).Returns(expected);

        var actual = await etpkMatchService.GetMaxBetETPK(32, 200);

        DeepAssert.Equal(expected, actual);
    }

    [Fact]
    public async Task GetMaxBetETPK_Always_ReturnMaxBetInfoET()
    {
        maxbetRepository.GetMatchInfoByAdminSiteId(Arg.Any<int>()).Returns(new MatchInfo
        {
            owpLeagueName = "Liam",
            owpMatchId = 3
        });

        var expected = new MaxbetInfo { AdminsiteID = 1, Maxbet = new MaxbetSpread { FirstHalfHandicap = 10000 } };

        specialProductService.GetMaxbetInfoET(Arg.Any<int>(), Arg.Any<int>()).Returns(expected);

        var actual = await etpkMatchService.GetMaxBetETPK(32, 2500);

        DeepAssert.Equal(expected, actual);
    }


    [InlineData(100)]
    [InlineData(200)]
    [Theory]
    public async Task GetMaxBetETPK_Always_ReturnMaxBetFutsalET(int subtype)
    {
        maxbetRepository.GetMatchInfoByAdminSiteId(Arg.Any<int>()).Returns(new MatchInfo
        {
            owpLeagueName = "FUTSAL V-League",
            owpMatchId = 3
        });
        var expected = new MaxbetInfo { AdminsiteID = 1, Maxbet = new MaxbetSpread { FirstHalfHandicap = 10000 } };
        futsalMatchService.GetMaxBetInfoFutsalET(Arg.Any<int>(), Arg.Any<int>()).Returns(expected);
        futsalMatchService.GetMaxBetInfoFutsalPEN(Arg.Any<int>(), Arg.Any<int>()).Returns(expected);

        var actual = await etpkMatchService.GetMaxBetETPK(32, subtype);

        DeepAssert.Equal(expected, actual);
    }


    [InlineData(100)]
    [InlineData(200)]
    [Theory]
    public async Task GetMaxBetETPK_Always_ReturnMaxBetMainMatchETPK(int subtype)
    {
        maxbetRepository.GetMatchInfoByAdminSiteId(Arg.Any<int>()).Returns(new MatchInfo
        {
            owpLeagueName = "DucPho League",
            owpMatchId = 3
        });
        var expected = new MaxbetInfo { AdminsiteID = 1, Maxbet = new MaxbetSpread { FirstHalfHandicap = 10000 } };
        mainMatchService.GetMaxbetInfoET(Arg.Any<int>(), Arg.Any<int>()).Returns(expected);
        mainMatchService.GetMaxbetInfoPEN(Arg.Any<int>(), Arg.Any<int>()).Returns(expected);

        var actual = await etpkMatchService.GetMaxBetETPK(32, subtype);

        DeepAssert.Equal(expected, actual);
    }

}

