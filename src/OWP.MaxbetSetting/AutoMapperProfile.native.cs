namespace OWP.MaxbetSetting;

using AutoMapper;
using Models;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<LogModelDeserialize, MaxBetLogSettingModel>();
    }
}
