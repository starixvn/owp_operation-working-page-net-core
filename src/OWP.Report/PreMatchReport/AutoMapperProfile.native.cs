namespace OWP.Report.PreMatchReport;

using AutoMapper;
using Extensions;
using Models;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        ExportPreMatchModelMap();
    }

    private void ExportPreMatchModelMap()
    {
        var mapper = CreateMap<ReportModel, ExportPreMatchModel>();
        ExportLeagueModelMap(mapper);
        ExportMatchModelMap(mapper);
    }

    private void ExportLeagueModelMap(IMappingExpression<ReportModel, ExportPreMatchModel> mapper)
    {
        mapper.ForMember(dest => dest.TotalLeague, act => act.MapFrom(src => src.TotalLeague.ToDashes()))
             .ForMember(dest => dest.TraderName, act => act.MapFrom(src => src.TraderName))
             .ForMember(dest => dest.LeagueHdpMj, act => act.MapFrom(src => src.ReportHDPLeague.MJ.ToDashes()))
             .ForMember(dest => dest.LeagueHdpM1, act => act.MapFrom(src => src.ReportHDPLeague.M1.ToDashes()))
             .ForMember(dest => dest.LeagueHdpM2, act => act.MapFrom(src => src.ReportHDPLeague.M2.ToDashes()))
             .ForMember(dest => dest.LeagueHdpM3, act => act.MapFrom(src => src.ReportHDPLeague.M3.ToDashes()))
             .ForMember(dest => dest.LeagueHdpM4, act => act.MapFrom(src => src.ReportHDPLeague.M4.ToDashes()))
             .ForMember(dest => dest.LeagueHdpOthers, act => act.MapFrom(src => src.ReportHDPLeague.Others.ToDashes()))
             .ForMember(dest => dest.LeagueHdpOthers1, act => act.MapFrom(src => src.ReportHDPLeague.Others1.ToDashes()))
             .ForMember(dest => dest.LeagueHdpOthers2, act => act.MapFrom(src => src.ReportHDPLeague.Others2.ToDashes()))
             .ForMember(dest => dest.LeagueHdpOthers3, act => act.MapFrom(src => src.ReportHDPLeague.Others3.ToDashes()))
             .ForMember(dest => dest.LeagueHdpOthers4, act => act.MapFrom(src => src.ReportHDPLeague.Others4.ToDashes()))
             .ForMember(dest => dest.LeagueHdpOthers5, act => act.MapFrom(src => src.ReportHDPLeague.Others5.ToDashes()))
             .ForMember(dest => dest.LeagueHdpUnknown, act => act.MapFrom(src => src.ReportHDPLeague.Unknown.ToDashes()))
             .ForMember(dest => dest.LeagueHdpMjT1, act => act.MapFrom(src => src.ReportHDPLeague.MjT1.ToDashes()))
             .ForMember(dest => dest.LeagueHdpMjT2, act => act.MapFrom(src => src.ReportHDPLeague.MjT2.ToDashes()))
             .ForMember(dest => dest.LeagueHdpClMjQ, act => act.MapFrom(src => src.ReportHDPLeague.ClMjQ.ToDashes()))
             .ForMember(dest => dest.LeagueHdpClMnQ, act => act.MapFrom(src => src.ReportHDPLeague.ClMnQ.ToDashes()))
             .ForMember(dest => dest.LeagueHdpIntMj, act => act.MapFrom(src => src.ReportHDPLeague.IntMj.ToDashes()))
             .ForMember(dest => dest.LeagueHdpIntMn, act => act.MapFrom(src => src.ReportHDPLeague.IntMn.ToDashes()))
             .ForMember(dest => dest.LeagueHdpWorldCup, act => act.MapFrom(src => src.ReportHDPLeague.WorldCup.ToDashes()))
             .ForMember(dest => dest.LeagueHdpEuro, act => act.MapFrom(src => src.ReportHDPLeague.Euro.ToDashes()))
             .ForMember(dest => dest.LeagueHdpTotal, act => act.MapFrom(src => src.ReportHDPLeague.Total.ToDashes()))
             .ForMember(dest => dest.LeagueOuMj, act => act.MapFrom(src => src.ReportOULeague.MJ.ToDashes()))
             .ForMember(dest => dest.LeagueOuM1, act => act.MapFrom(src => src.ReportOULeague.M1.ToDashes()))
             .ForMember(dest => dest.LeagueOuM2, act => act.MapFrom(src => src.ReportOULeague.M2.ToDashes()))
             .ForMember(dest => dest.LeagueOuM3, act => act.MapFrom(src => src.ReportOULeague.M3.ToDashes()))
             .ForMember(dest => dest.LeagueOuM4, act => act.MapFrom(src => src.ReportOULeague.M4.ToDashes()))
             .ForMember(dest => dest.LeagueOuOthers, act => act.MapFrom(src => src.ReportOULeague.Others.ToDashes()))
             .ForMember(dest => dest.LeagueOuOthers1, act => act.MapFrom(src => src.ReportOULeague.Others1.ToDashes()))
             .ForMember(dest => dest.LeagueOuOthers2, act => act.MapFrom(src => src.ReportOULeague.Others2.ToDashes()))
             .ForMember(dest => dest.LeagueOuOthers3, act => act.MapFrom(src => src.ReportOULeague.Others3.ToDashes()))
             .ForMember(dest => dest.LeagueOuOthers4, act => act.MapFrom(src => src.ReportOULeague.Others4.ToDashes()))
             .ForMember(dest => dest.LeagueOuOthers5, act => act.MapFrom(src => src.ReportOULeague.Others5.ToDashes()))
             .ForMember(dest => dest.LeagueOuUnknown, act => act.MapFrom(src => src.ReportOULeague.Unknown.ToDashes()))
             .ForMember(dest => dest.LeagueOuMjT1, act => act.MapFrom(src => src.ReportOULeague.MjT1.ToDashes()))
             .ForMember(dest => dest.LeagueOuMjT2, act => act.MapFrom(src => src.ReportOULeague.MjT2.ToDashes()))
             .ForMember(dest => dest.LeagueOuClMjQ, act => act.MapFrom(src => src.ReportOULeague.ClMjQ.ToDashes()))
             .ForMember(dest => dest.LeagueOuClMnQ, act => act.MapFrom(src => src.ReportOULeague.ClMnQ.ToDashes()))
             .ForMember(dest => dest.LeagueOuIntMj, act => act.MapFrom(src => src.ReportOULeague.IntMj.ToDashes()))
             .ForMember(dest => dest.LeagueOuIntMn, act => act.MapFrom(src => src.ReportOULeague.IntMn.ToDashes()))
             .ForMember(dest => dest.LeagueOuWorldCup, act => act.MapFrom(src => src.ReportOULeague.WorldCup.ToDashes()))
             .ForMember(dest => dest.LeagueOuEuro, act => act.MapFrom(src => src.ReportOULeague.Euro.ToDashes()))
             .ForMember(dest => dest.LeagueOuTotal, act => act.MapFrom(src => src.ReportOULeague.Total.ToDashes()));
    }

    private void ExportMatchModelMap(IMappingExpression<ReportModel, ExportPreMatchModel> mapper)
    {
        mapper
          .ForMember(dest => dest.TotalMatches, act => act.MapFrom(src => src.TotalMatches.ToDashes()))
          .ForMember(dest => dest.MatchHdpMj, act => act.MapFrom(src => src.ReportHDPMatches.MJ.ToDashes()))
          .ForMember(dest => dest.MatchHdpM1, act => act.MapFrom(src => src.ReportHDPMatches.M1.ToDashes()))
          .ForMember(dest => dest.MatchHdpM2, act => act.MapFrom(src => src.ReportHDPMatches.M2.ToDashes()))
          .ForMember(dest => dest.MatchHdpM3, act => act.MapFrom(src => src.ReportHDPMatches.M3.ToDashes()))
          .ForMember(dest => dest.MatchHdpM4, act => act.MapFrom(src => src.ReportHDPMatches.M4.ToDashes()))
          .ForMember(dest => dest.MatchHdpOthers, act => act.MapFrom(src => src.ReportHDPMatches.Others.ToDashes()))
          .ForMember(dest => dest.MatchHdpOthers1, act => act.MapFrom(src => src.ReportHDPMatches.Others1.ToDashes()))
          .ForMember(dest => dest.MatchHdpOthers2, act => act.MapFrom(src => src.ReportHDPMatches.Others2.ToDashes()))
          .ForMember(dest => dest.MatchHdpOthers3, act => act.MapFrom(src => src.ReportHDPMatches.Others3.ToDashes()))
          .ForMember(dest => dest.MatchHdpOthers4, act => act.MapFrom(src => src.ReportHDPMatches.Others4.ToDashes()))
          .ForMember(dest => dest.MatchHdpOthers5, act => act.MapFrom(src => src.ReportHDPMatches.Others5.ToDashes()))
          .ForMember(dest => dest.MatchHdpUnknown, act => act.MapFrom(src => src.ReportHDPMatches.Unknown.ToDashes()))
          .ForMember(dest => dest.MatchHdpMjT1, act => act.MapFrom(src => src.ReportHDPMatches.MjT1.ToDashes()))
          .ForMember(dest => dest.MatchHdpMjT2, act => act.MapFrom(src => src.ReportHDPMatches.MjT2.ToDashes()))
          .ForMember(dest => dest.MatchHdpClMjQ, act => act.MapFrom(src => src.ReportHDPMatches.ClMjQ.ToDashes()))
          .ForMember(dest => dest.MatchHdpClMnQ, act => act.MapFrom(src => src.ReportHDPMatches.ClMnQ.ToDashes()))
          .ForMember(dest => dest.MatchHdpIntMj, act => act.MapFrom(src => src.ReportHDPMatches.IntMj.ToDashes()))
          .ForMember(dest => dest.MatchHdpIntMn, act => act.MapFrom(src => src.ReportHDPMatches.IntMn.ToDashes()))
          .ForMember(dest => dest.MatchHdpWorldCup, act => act.MapFrom(src => src.ReportHDPMatches.WorldCup.ToDashes()))
          .ForMember(dest => dest.MatchHdpEuro, act => act.MapFrom(src => src.ReportHDPMatches.Euro.ToDashes()))
          .ForMember(dest => dest.MatchHdpTotal, act => act.MapFrom(src => src.ReportHDPMatches.Total.ToDashes()))
          .ForMember(dest => dest.MatchOuMj, act => act.MapFrom(src => src.ReportOUMatches.MJ.ToDashes()))
          .ForMember(dest => dest.MatchOuM1, act => act.MapFrom(src => src.ReportOUMatches.M1.ToDashes()))
          .ForMember(dest => dest.MatchOuM2, act => act.MapFrom(src => src.ReportOUMatches.M2.ToDashes()))
          .ForMember(dest => dest.MatchOuM3, act => act.MapFrom(src => src.ReportOUMatches.M3.ToDashes()))
          .ForMember(dest => dest.MatchOuM4, act => act.MapFrom(src => src.ReportOUMatches.M4.ToDashes()))
          .ForMember(dest => dest.MatchOuOthers, act => act.MapFrom(src => src.ReportOUMatches.Others.ToDashes()))
          .ForMember(dest => dest.MatchOuOthers1, act => act.MapFrom(src => src.ReportOUMatches.Others1.ToDashes()))
          .ForMember(dest => dest.MatchOuOthers2, act => act.MapFrom(src => src.ReportOUMatches.Others2.ToDashes()))
          .ForMember(dest => dest.MatchOuOthers3, act => act.MapFrom(src => src.ReportOUMatches.Others3.ToDashes()))
          .ForMember(dest => dest.MatchOuOthers4, act => act.MapFrom(src => src.ReportOUMatches.Others4.ToDashes()))
          .ForMember(dest => dest.MatchOuOthers5, act => act.MapFrom(src => src.ReportOUMatches.Others5.ToDashes()))
          .ForMember(dest => dest.MatchOuUnknown, act => act.MapFrom(src => src.ReportOUMatches.Unknown.ToDashes()))
          .ForMember(dest => dest.MatchOuMjT1, act => act.MapFrom(src => src.ReportOUMatches.MjT1.ToDashes()))
          .ForMember(dest => dest.MatchOuMjT2, act => act.MapFrom(src => src.ReportOUMatches.MjT2.ToDashes()))
          .ForMember(dest => dest.MatchOuClMjQ, act => act.MapFrom(src => src.ReportOUMatches.ClMjQ.ToDashes()))
          .ForMember(dest => dest.MatchOuClMnQ, act => act.MapFrom(src => src.ReportOUMatches.ClMnQ.ToDashes()))
          .ForMember(dest => dest.MatchOuIntMj, act => act.MapFrom(src => src.ReportOUMatches.IntMj.ToDashes()))
          .ForMember(dest => dest.MatchOuIntMn, act => act.MapFrom(src => src.ReportOUMatches.IntMn.ToDashes()))
          .ForMember(dest => dest.MatchOuWorldCup, act => act.MapFrom(src => src.ReportOUMatches.WorldCup.ToDashes()))
          .ForMember(dest => dest.MatchOuEuro, act => act.MapFrom(src => src.ReportOUMatches.Euro.ToDashes()))
          .ForMember(dest => dest.MatchOuTotal, act => act.MapFrom(src => src.ReportOUMatches.Total.ToDashes()));
    }
}