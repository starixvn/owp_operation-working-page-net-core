

namespace OWP.EventPicking;
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<socer_event_change_game_date, InsertOrUpdateEventChangeKoRequest>()
            .ConstructUsing(x => new InsertOrUpdateEventChangeKoRequest(x.match_id, x.new_game_date));
    }
}
