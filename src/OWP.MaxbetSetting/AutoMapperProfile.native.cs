using AutoMapper;
using OWP.MaxbetSetting.Models;

namespace OWP.MaxbetSetting;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<LogModelDeserialize, MaxBetLogSettingModel>();
    }
}
