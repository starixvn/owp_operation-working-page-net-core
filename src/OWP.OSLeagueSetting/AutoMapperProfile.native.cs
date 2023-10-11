using AutoMapper;
using OWP.OSLeagueSetting.Criteria;
using OWP.OSLeagueSetting.Handlers;
using OWP.OSLeagueSetting.Models;

namespace OWP.OSLeagueSetting;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<LeagueSettingInfo, LeagueSettingModel>();
        CreateMap<SettingDetail, AddSettingCriteria>();
        CreateMap<SettingDetail, EditSettingCriteria>();
        CreateMap<SettingModelDeserialize, LogDataLeagueSetting>();
        CreateMap<ExportLeagueSettingRequest, GetLeagueInfoHandler.Request>();
        CreateMap<LeagueSettingInfo, LeagueSettingExport>();
    }
}