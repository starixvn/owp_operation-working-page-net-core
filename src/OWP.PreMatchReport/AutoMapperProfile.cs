using AutoMapper;
using OWP.PreMatchReport.Models;

namespace OWP.PreMatchReport;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<ExportPreMatchModel, ReportModel>();
    }
}