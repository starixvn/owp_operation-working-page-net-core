using AutoMapper;
using OWP.NewLeagueSetting.Command;


namespace OWP.NewLeagueSetting;
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<InsertLeagueSettingRequest, InsertLeagueSettingCriteria>();
    }
}
