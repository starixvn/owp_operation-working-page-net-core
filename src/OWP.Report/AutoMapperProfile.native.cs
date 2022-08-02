namespace OWP.Report
{
    using AutoMapper;
    using LAReport.Models;
    using TraderReport.Models;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ReportModel, ExcelReportModel>();
            CreateMap<LAReportModel, LAExcelReportModel>();
        }
    }
}