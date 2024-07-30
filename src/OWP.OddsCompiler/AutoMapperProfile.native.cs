
namespace OWP.OddsCompiler;
using AutoMapper;
using Models;
using System.Linq;

public class AutoMapperProfile : Profile
{
    private const int home = 1;

    private const int away = 2;

    private const int hdpBettype = 1;

    private const int ouBettype = 3;

    public AutoMapperProfile()
    {
        CreateMap<MatchInfoModel, ExcelReportModel>()
            .ForMember(dest => dest.KickOffTime, opt => opt.MapFrom(src => src.KickOffTimeToDisplay))
            .ForMember(dest => dest.OtherTime, opt => opt.MapFrom(src => src.OtherTimeToDisplay))
            .ForMember(dest => dest.Fav, opt => opt.MapFrom(src => GetTeamChoice(src)))
            .ForMember(dest => dest.HDP, opt => opt.MapFrom(src => src.MatchOddsRow.Handicap.HDP))
            .ForMember(dest => dest.HomeOdds, opt => opt.MapFrom(src => src.MatchOddsRow.Handicap.Odds1))
            .ForMember(dest => dest.AwayOdds, opt => opt.MapFrom(src => src.MatchOddsRow.Handicap.Odds2))
            .ForMember(dest => dest.OU, opt => opt.MapFrom(src => src.MatchOddsRow.OverUnder.HDP))
            .ForMember(dest => dest.Over, opt => opt.MapFrom(src => src.MatchOddsRow.OverUnder.Odds1))
            .ForMember(dest => dest.Under, opt => opt.MapFrom(src => src.MatchOddsRow.OverUnder.Odds2))
            .ForMember(dest => dest.One, opt => opt.MapFrom(src => src.MatchOddsRow.OneXTwo.Odds1))
            .ForMember(dest => dest.Two, opt => opt.MapFrom(src => src.MatchOddsRow.OneXTwo.Odds2))
            .ForMember(dest => dest.X, opt => opt.MapFrom(src => src.MatchOddsRow.OneXTwo.Odds3))
            .ForMember(dest => dest.MaxBetHDP, opt => opt.MapFrom((src, _) => GetMB(src, hdpBettype)))
            .ForMember(dest => dest.SpreadHDP, opt => opt.MapFrom((src, _) => GetSpread(src, hdpBettype)))
            .ForMember(dest => dest.SpreadOU, opt => opt.MapFrom((src, _) => GetSpread(src, ouBettype)));
    }

    private static string GetMB(MatchInfoModel src, int bettype)
    {
        var matchSetting = src.MatchSettings.FirstOrDefault(x => x.Bettype == bettype);
        return matchSetting != null ? matchSetting.MaxBetToDisplay : string.Empty;
    }

    private static string GetSpread(MatchInfoModel src, int bettype)
    {
        var matchSetting = src.MatchSettings.FirstOrDefault(x => x.Bettype == bettype);
        return matchSetting != null ? matchSetting.SpreadToDisplay : string.Empty;
    }

    private static string GetTeamChoice(MatchInfoModel src)
    {
        switch (src.MatchOddsRow.Handicap.TeamChoice)
        {
            case home:
                return "Home";
            case away:
                return "Away";
            default:
                return string.Empty;
        }
    }
}