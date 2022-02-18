using AutoMapper;
using OWP.OvertimeReport.Handlers;
using OWP.OvertimeReport.Models;

namespace OWP.OvertimeReport
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<QueryReportByMonthRequest, OverTimeRequest>();
            CreateMap<QueryReportByYearRequest, OverTimeRequest>();
            CreateMap<ExportReportByMonthRequest, OverTimeRequest>();
            CreateMap<ExportReportByYearRequest, QueryReportByYearRequest>();
        }
    }
}