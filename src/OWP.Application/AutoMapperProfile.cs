using AutoMapper;
using OWP.Application.Message;

namespace OWP.Application;
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<QueryModel, QueryCriteria>();
    }
}
