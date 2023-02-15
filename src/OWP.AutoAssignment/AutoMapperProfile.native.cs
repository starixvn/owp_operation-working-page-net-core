namespace OWP.AutoAssignment;

using AutoMapper;
using Models.InternalApi;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<InfoSiteMatchModel, ExportMatchModel>();
    }
}