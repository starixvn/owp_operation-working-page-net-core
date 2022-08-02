using AutoMapper;
using Fanex.Data.Repository;
using NSubstitute;
using OWP.OSLeagueSetting.Handlers;
using OWP.OSLeagueSetting.Models;
using OWP.OSLeagueSetting.Services;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.OSLeagueSetting.UnitTests.Services;

public class GetLeagueSettingServiceUnitTest
{
    private readonly IMapper mapper;
    private readonly Repository repository;

    public GetLeagueSettingServiceUnitTest()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<LeagueSettingInfo, LeagueSettingModel>();
        });

        mapper = configuration.CreateMapper();
        repository = Substitute.For<Repository>();
    }

    [Fact]
    public async void GetLeagueSetting_Always_ListLeagueSettingModel()
    {
        var service = new GetLeagueSettingServiceImp(repository, mapper);
        repository.GetLeagueSettingInfo(Arg.Any<GetLeagueInfoHandler.Request>()).Returns(GetLeagueSettingInfosRaw());
        var expected = GetExpected();

        var actual = await service.GetLeagueSetting(new GetLeagueInfoHandler.Request());

        DeepAssert.Equal(expected, actual);
    }

    private IList<LeagueSettingModel> GetExpected()
        => new List<LeagueSettingModel>
        {
            new()
            {
                LeagueSettingId = 1,
                OSLeagueName = "C1",
                MaxBetHDP = 40,
                MaxBetOU = 40,
                RoundName= string.Empty,
                SummaryInfo = new List<SummaryDetail>(),
                SummaryRemark = new List<SummaryDetail>{new (){Text = "00:03 - 00:04", Style = "time-range" } }
            },
            new()
            {
                LeagueSettingId = 3,
                OSLeagueName = "C2",
                MaxBetHDP = 50,
                MaxBetOU = 40,
                RoundName= string.Empty,
                SummaryInfo = new List<SummaryDetail>(),
                SummaryRemark = new List<SummaryDetail>(),
            },
            new()
            {
                LeagueSettingId = 2,
                OSLeagueName = "C2",
                MaxBetHDP = 40,
                MaxBetOU = 40,
                RoundName= string.Empty,
                SummaryInfo = new List<SummaryDetail>(),
                SummaryRemark = new List<SummaryDetail>{new (){Text = "00:03 - ", Style = "time-range" } }
            }
        };

    private IList<LeagueSettingInfo> GetLeagueSettingInfosRaw()
        => new List<LeagueSettingInfo>
        {
            new()
            {
                LeagueSettingId = 1,
                OSLeagueName = "C1",
                MaxBetHDP = 40,
                MaxBetOU = 40,
                KOFromTime = new TimeSpan(0,3,20),
                KOToTime = new TimeSpan(0,4,20)
            },
            new()
            {
                LeagueSettingId = 1,
                OSLeagueName = "C1",
                MaxBetHDP = 40,
                MaxBetOU = 40,
                KOFromTime = new TimeSpan(0,3,20),
                KOToTime = new TimeSpan(0,4,20)
             },
            new()
            {
                LeagueSettingId = 2,
                OSLeagueName = "C2",
                MaxBetHDP = 40,
                MaxBetOU = 40,
                KOFromTime = new TimeSpan(0,3,20),
                KOToTime = null
            },
            new()
            {
                LeagueSettingId = 2,
                OSLeagueName = "C2",
                MaxBetHDP = 40,
                MaxBetOU = 40,
                KOFromTime = new TimeSpan(0,3,20),
                KOToTime = null
            },
            new()
            {
                LeagueSettingId = 3,
                OSLeagueName = "C2",
                MaxBetHDP = 50,
                MaxBetOU = 40,
                KOFromTime = null,
                KOToTime = null,
            },
            new()
            {
                LeagueSettingId = 3,
                OSLeagueName = "C2",
                MaxBetHDP = 50,
                MaxBetOU = 40,
                KOFromTime = null,
                KOToTime = null
            }
        };
}