namespace OWP.Report.OvertimeReport;

using AutoMapper;
using Handlers;
using Models;

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