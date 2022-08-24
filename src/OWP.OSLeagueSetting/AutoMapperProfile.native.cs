namespace OWP.OSLeagueSetting;

using Models;
using AutoMapper;
using Criteria;

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