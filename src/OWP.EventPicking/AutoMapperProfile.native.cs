using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace OWP.EventPicking;
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<socer_event_change_game_date, InsertOrUpdateEventChangeKoRequest>()
            .ForMember(dest => dest.InfositeId, opt => opt.MapFrom(src => src.match_id))
            .ForMember(dest => dest.NewKO, opt => opt.MapFrom(src => src.new_game_date));
    }
}
