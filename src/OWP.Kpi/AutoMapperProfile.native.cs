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
        .ForMember(dest => dest.AchievedGames.VirtualMatches, opt => opt.MapFrom(ps => ps.VirtualMatches))
        .ForMember(dest => dest.AchievedGames.MultipleMatches, opt => opt.MapFrom(ps => ps.MultipleMatches))
        .ForMember(dest => dest.AchievedGames.SpecialProducts, opt => opt.MapFrom(ps => ps.SpecialProducts))
        .ForMember(dest => dest.AchievedGames.FiveMajorLeagues, opt => opt.MapFrom(ps => ps.FiveMajorLeagues))
        .ForMember(dest => dest.AchievedGames.MultipleProducts, opt => opt.MapFrom(ps => ps.MultipleProducts));
    }
}