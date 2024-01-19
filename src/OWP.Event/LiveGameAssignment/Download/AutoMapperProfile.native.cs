using AutoMapper;
using OWP.Event.LiveGameAssignment.Download.Models.EventPicking;

namespace OWP.Event.LiveGameAssignment.Download;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<TVList, Condition>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Label));
    }
}