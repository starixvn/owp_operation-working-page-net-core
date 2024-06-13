using AutoMapper;
using OWP.MessageManagement.Criteria;
using OWP.MessageManagement.Models;


namespace OWP.MessageManagement;
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<MessageBoxQueryParam, MessageBoxQueryCriteria>();
    }
}
