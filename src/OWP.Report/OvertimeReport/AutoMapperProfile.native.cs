using AutoMapper;
using OWP.Report.OvertimeReport.Handlers;
using OWP.Report.OvertimeReport.Models;

namespace OWP.Report.OvertimeReport;

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