using AutoMapper;
using OWP.AutoAssignment.Models.InternalApi;

namespace OWP.AutoAssignment
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<InfoSiteMatchModel, ExportMatchModel>();
        }
    }
}