using AutoMapper;
using OWP.Kpi.LA.Models;

namespace OWP.Kpi;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<KpiInfo, KpiInfoViewModel>()
        .ForMember(dest => dest.UserId, opt => opt.MapFrom(ps => ps.UserId))
        .ForMember(dest => dest.UserName, opt => opt.MapFrom(ps => ps.UserName))
        .ForPath(dest => dest.AchievedGames.VirtualMatches, opt => opt.MapFrom(ps => ps.VirtualMatches))
        .ForPath(dest => dest.AchievedGames.MultipleMatches, opt => opt.MapFrom(ps => ps.MultipleMatches))
        .ForPath(dest => dest.AchievedGames.SpecialProducts, opt => opt.MapFrom(ps => ps.SpecialProducts))
        .ForPath(dest => dest.AchievedGames.FiveMajorLeagues, opt => opt.MapFrom(ps => ps.FiveMajorLeagues))
        .ForPath(dest => dest.AchievedGames.MultipleProducts, opt => opt.MapFrom(ps => ps.MultipleProducts));
    }
}