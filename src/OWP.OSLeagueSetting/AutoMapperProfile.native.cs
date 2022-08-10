namespace OWP.OSLeagueSetting;

using Models;
using AutoMapper;
using OWP.OSLeagueSetting.Criteria;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<LeagueSettingInfo, LeagueSettingModel>();
        CreateMap<SettingDetail, AddSettingCriteria>();
        CreateMap<SettingDetail, EditSettingCriteria>();
        CreateMap<LogModelDeserialize, NewDataLeagueSetting>();
        CreateMap<LogModelDeserialize, OldDataLeagueSetting>();
    }
}