using AutoMapper;
using OWP.Report.LAReport.Models;
using OWP.Report.TraderReport.Models;

namespace OWP.Report;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<ReportModel, ExcelReportModel>();
        CreateMap<LAReportModel, LAExcelReportModel>();
    }
}