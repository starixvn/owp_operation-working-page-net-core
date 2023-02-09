namespace OWP.OSLeagueSetting;

using AutoMapper;
using Criteria;
using Models;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<LeagueSettingInfo, LeagueSettingModel>();
        CreateMap<SettingDetail, AddSettingCriteria>();
        CreateMap<SettingDetail, EditSettingCriteria>();
        CreateMap<SettingModelDeserialize, LogDataLeagueSetting>();
    }
}