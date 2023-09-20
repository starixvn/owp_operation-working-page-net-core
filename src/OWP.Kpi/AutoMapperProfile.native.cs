using AutoMapper;
using OWP.Kpi.LA.Models;
using OWP.Kpi.LA.ViewModels;

namespace OWP.Kpi;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<KpiInfo, KpiInfoViewModel>()
        .ForMember(dest => dest.UserId, opt => opt.MapFrom(ps => ps.UserId))
        .ForMember(dest => dest.UserName, opt => opt.MapFrom(ps => ps.UserName))
        .ForMember(dest => dest.Target, opt => opt.MapFrom(ps => ps.Target))
        .ForMember(dest => dest.TargetId, opt => opt.MapFrom(ps => ps.TargetId))
        .ForPath(dest => dest.AchievedGames.VirtualMatches, opt => opt.MapFrom(ps => ps.VirtualMatches))
        .ForPath(dest => dest.AchievedGames.MultipleMatches, opt => opt.MapFrom(ps => ps.MultipleMatches))
        .ForPath(dest => dest.AchievedGames.SpecialProducts, opt => opt.MapFrom(ps => ps.SpecialProducts))
        .ForPath(dest => dest.AchievedGames.FiveMajorLeagues, opt => opt.MapFrom(ps => ps.FiveMajorLeagues))
        .ForPath(dest => dest.AchievedGames.MultipleProducts, opt => opt.MapFrom(ps => ps.MultipleProducts));

        CreateMap<KpiInfoViewModel, KpiInfoReport>()
            .ForMember(dest => dest.AchievedVirtualMatches, opt => opt.MapFrom(src => src.AchievedGames.VirtualMatches))
            .ForMember(dest => dest.AchievedMultipleMatches, opt => opt.MapFrom(src => src.AchievedGames.MultipleMatches))
            .ForMember(dest => dest.AchievedSpecialProducts, opt => opt.MapFrom(src => src.AchievedGames.SpecialProducts))
            .ForMember(dest => dest.AchievedFiveMajorLeagues, opt => opt.MapFrom(src => src.AchievedGames.FiveMajorLeagues))
            .ForMember(dest => dest.AchievedMultipleProducts, opt => opt.MapFrom(src => src.AchievedGames.MultipleProducts))
            .ForMember(dest => dest.GapVirtualMatches, opt => opt.MapFrom(src => src.GapWithTarget.VirtualMatches))
            .ForMember(dest => dest.GapMultipleMatches, opt => opt.MapFrom(src => src.GapWithTarget.MultipleMatches))
            .ForMember(dest => dest.GapSpecialProducts, opt => opt.MapFrom(src => src.GapWithTarget.SpecialProducts))
            .ForMember(dest => dest.GapFiveMajorLeagues, opt => opt.MapFrom(src => src.GapWithTarget.FiveMajorLeagues))
            .ForMember(dest => dest.GapMultipleProducts, opt => opt.MapFrom(src => src.GapWithTarget.MultipleProducts));
    }
}