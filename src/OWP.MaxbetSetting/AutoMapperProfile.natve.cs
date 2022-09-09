namespace OWP.MaxbetSetting;

using Models;
using AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<LogModelDeserialize, MaxBetLogSettingModel>();
        CreateMap<LogModelDeserialize, MaxBetLogSettingModel>();
    }
}
