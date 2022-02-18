using NSubstitute;
using Xunit;
using Xunit.Asserts.Compare;
using AutoMapper;

using OWP.PreMatchReport.Models;
using OWP.PreMatchReport.Services;



namespace OWP.PreMatchReport.UnitTests.Services;

public class GetPreMatchReportServiceUnitTests
{
    private readonly Repository repository;
    private readonly IMapper mapper;
    private readonly GetPreMatchReportService service;

    public GetPreMatchReportServiceUnitTests()
    {
        repository = Substitute.For<Repository>();
        var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReportModel, ExportPreMatchModel>()
                .ForMember(dest => dest.TotalLeague, act => act.MapFrom(src => src.TotalLeague))
                .ForMember(dest => dest.TotalMatches, act => act.MapFrom(src => src.TotalMatches))
                .ForMember(dest => dest.TraderName, act => act.MapFrom(src => src.TraderName))
                //
                .ForMember(dest => dest.LeagueHdpMj, act => act.MapFrom(src => src.ReportHDPLeague.MJ))
                .ForMember(dest => dest.LeagueHdpM1, act => act.MapFrom(src => src.ReportHDPLeague.M1))
                .ForMember(dest => dest.LeagueHdpM2, act => act.MapFrom(src => src.ReportHDPLeague.M2))
                .ForMember(dest => dest.LeagueHdpM3, act => act.MapFrom(src => src.ReportHDPLeague.M3))
                .ForMember(dest => dest.LeagueHdpM4, act => act.MapFrom(src => src.ReportHDPLeague.M4))
                .ForMember(dest => dest.LeagueHdpOthers, act => act.MapFrom(src => src.ReportHDPLeague.Others))
                .ForMember(dest => dest.LeagueHdpOthers1, act => act.MapFrom(src => src.ReportHDPLeague.Others1))
                .ForMember(dest => dest.LeagueHdpOthers2, act => act.MapFrom(src => src.ReportHDPLeague.Others2))
                .ForMember(dest => dest.LeagueHdpOthers3, act => act.MapFrom(src => src.ReportHDPLeague.Others3))
                .ForMember(dest => dest.LeagueHdpOthers4, act => act.MapFrom(src => src.ReportHDPLeague.Others4))
                .ForMember(dest => dest.LeagueHdpOthers5, act => act.MapFrom(src => src.ReportHDPLeague.Others5))
                .ForMember(dest => dest.LeagueHdpUnknown, act => act.MapFrom(src => src.ReportHDPLeague.Unknown))
                .ForMember(dest => dest.LeagueHdpTotal, act => act.MapFrom(src => src.ReportHDPLeague.Total))
                //
                .ForMember(dest => dest.LeagueOuMj, act => act.MapFrom(src => src.ReportOULeague.MJ))
                .ForMember(dest => dest.LeagueOuM1, act => act.MapFrom(src => src.ReportOULeague.M1))
                .ForMember(dest => dest.LeagueOuM2, act => act.MapFrom(src => src.ReportOULeague.M2))
                .ForMember(dest => dest.LeagueOuM3, act => act.MapFrom(src => src.ReportOULeague.M3))
                .ForMember(dest => dest.LeagueOuM4, act => act.MapFrom(src => src.ReportOULeague.M4))
                .ForMember(dest => dest.LeagueOuOthers, act => act.MapFrom(src => src.ReportOULeague.Others))
                .ForMember(dest => dest.LeagueOuOthers1, act => act.MapFrom(src => src.ReportOULeague.Others1))
                .ForMember(dest => dest.LeagueOuOthers2, act => act.MapFrom(src => src.ReportOULeague.Others2))
                .ForMember(dest => dest.LeagueOuOthers3, act => act.MapFrom(src => src.ReportOULeague.Others3))
                .ForMember(dest => dest.LeagueOuOthers4, act => act.MapFrom(src => src.ReportOULeague.Others4))
                .ForMember(dest => dest.LeagueOuOthers5, act => act.MapFrom(src => src.ReportOULeague.Others5))
                .ForMember(dest => dest.LeagueOuUnknown, act => act.MapFrom(src => src.ReportOULeague.Unknown))
                .ForMember(dest => dest.LeagueOuTotal, act => act.MapFrom(src => src.ReportOULeague.Total))
                //
                .ForMember(dest => dest.MatchHdpMj, act => act.MapFrom(src => src.ReportHDPMatches.MJ))
                .ForMember(dest => dest.MatchHdpM1, act => act.MapFrom(src => src.ReportHDPMatches.M1))
                .ForMember(dest => dest.MatchHdpM2, act => act.MapFrom(src => src.ReportHDPMatches.M2))
                .ForMember(dest => dest.MatchHdpM3, act => act.MapFrom(src => src.ReportHDPMatches.M3))
                .ForMember(dest => dest.MatchHdpM4, act => act.MapFrom(src => src.ReportHDPMatches.M4))
                .ForMember(dest => dest.MatchHdpOthers, act => act.MapFrom(src => src.ReportHDPMatches.Others))
                .ForMember(dest => dest.MatchHdpOthers1, act => act.MapFrom(src => src.ReportHDPMatches.Others1))
                .ForMember(dest => dest.MatchHdpOthers2, act => act.MapFrom(src => src.ReportHDPMatches.Others2))
                .ForMember(dest => dest.MatchHdpOthers3, act => act.MapFrom(src => src.ReportHDPMatches.Others3))
                .ForMember(dest => dest.MatchHdpOthers4, act => act.MapFrom(src => src.ReportHDPMatches.Others4))
                .ForMember(dest => dest.MatchHdpOthers5, act => act.MapFrom(src => src.ReportHDPMatches.Others5))
                .ForMember(dest => dest.MatchHdpUnknown, act => act.MapFrom(src => src.ReportHDPMatches.Unknown))
                .ForMember(dest => dest.MatchHdpTotal, act => act.MapFrom(src => src.ReportHDPMatches.Total))
                //
                .ForMember(dest => dest.MatchOuMj, act => act.MapFrom(src => src.ReportOUMatches.MJ))
                .ForMember(dest => dest.MatchOuM1, act => act.MapFrom(src => src.ReportOUMatches.M1))
                .ForMember(dest => dest.MatchOuM2, act => act.MapFrom(src => src.ReportOUMatches.M2))
                .ForMember(dest => dest.MatchOuM3, act => act.MapFrom(src => src.ReportOUMatches.M3))
                .ForMember(dest => dest.MatchOuM4, act => act.MapFrom(src => src.ReportOUMatches.M4))
                .ForMember(dest => dest.MatchOuOthers, act => act.MapFrom(src => src.ReportOUMatches.Others))
                .ForMember(dest => dest.MatchOuOthers1, act => act.MapFrom(src => src.ReportOUMatches.Others1))
                .ForMember(dest => dest.MatchOuOthers2, act => act.MapFrom(src => src.ReportOUMatches.Others2))
                .ForMember(dest => dest.MatchOuOthers3, act => act.MapFrom(src => src.ReportOUMatches.Others3))
                .ForMember(dest => dest.MatchOuOthers4, act => act.MapFrom(src => src.ReportOUMatches.Others4))
                .ForMember(dest => dest.MatchOuOthers5, act => act.MapFrom(src => src.ReportOUMatches.Others5))
                .ForMember(dest => dest.MatchOuUnknown, act => act.MapFrom(src => src.ReportOUMatches.Unknown))
                .ForMember(dest => dest.MatchOuTotal, act => act.MapFrom(src => src.ReportOUMatches.Total));
            });

        mapper = configuration.CreateMapper();
        service = new GetPreMatchReportServiceImp(repository, mapper);
    }

    [Fact]
    public async Task GetPreMatchReport_Always_ShouldReturn()
    {
        var request = new QueryModel()
        {
            FromDate = DateTime.Parse("2021-12-08"),
            ToDate = DateTime.Parse("2021-12-08"),
            KeyWord = ""
        };

        repository.GetReportForLeague(request).Returns(MockReportForLeague());
        repository.GetReportForMatches(request).Returns(MockReportForMatches());

        var reportPrematch = new ReportModel()
        {
            TraderName = "sa",
            TotalMatches = 14,
            TotalLeague = 14,
            ReportHDPLeague = new ReportHDPLeagueModel()
            {
                MJ = 1,
                M1 = 1
            },
            ReportHDPMatches = new ReportHDPMatchesModel()
            {
                MJ = 1,
                M1 = 1
            }
        };

        var actual = await service.GetReports(request);

        var expected = Expected();

        Assert.Equal(expected[0].TotalMatches, actual[0].TotalMatches);
        Assert.Equal(expected[0].TotalLeague, actual[0].TotalLeague);
        Assert.Equal(expected[0].LeagueHdpMj, actual[0].LeagueHdpMj);
        Assert.Equal(expected[0].MatchHdpMj, actual[0].MatchHdpMj);
    }

    public List<ExportPreMatchModel> Expected()
    => new()
    {
        new()
        {
            TraderName = "sa",
            TotalMatches = "14",
            TotalLeague = "14",
            LeagueHdpMj = "7",
            LeagueHdpM1 = "7",
            MatchHdpMj = "7",
            MatchHdpM1 = "7"
        }
    };
    public List<QueryPrematchReportModel> MockReportForLeague()
    => new()
    {
        new()
        {
            TraderName = "sa",
            GroupId = 1,
            Bettype = 1,
            Amount = 7
        },
        new()
        {
            TraderName = "sa",
            GroupId = 2,
            Bettype = 1,
            Amount = 7
        }
    };
    public List<QueryPrematchReportModel> MockReportForMatches()
    => new()
    {
        new()
        {
            TraderName = "sa",
            GroupId = 1,
            Bettype = 1,
            Amount = 7
        },
        new()
        {
            TraderName = "sa",
            GroupId = 2,
            Bettype = 1,
            Amount = 7
        }
    };
}