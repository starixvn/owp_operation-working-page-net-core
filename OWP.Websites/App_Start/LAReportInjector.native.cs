using OWP.Report.LAReport.Services;
using OWP.Report.TraderReport.Services;
using OWP.Services;
using ExportExcelService = OWP.Report.TraderReport.Services.ExportExcelService;
using ExportExcelServiceImp = OWP.Report.TraderReport.Services.ExportExcelServiceImp;

namespace OWP.Websites.App_Start;

public static class LAReportInjector
{
    public static void AddLAReportService(this IServiceCollection services)
    {
        services.AddScoped<RepositoryService, RepositoryServiceImp>();
        services.AddScoped<QueryRepository, QueryRepositoryImp>();
        services.AddScoped<RichHttpClient, RichHttpClientImp>();
        services.AddScoped<TGRSReportService, TGRSReportServiceImp>();
        services.AddScoped<PermissionService, PermissionServiceImp>();
        services.AddScoped<ReportPermissionService, ReportPermissionServiceImp>();
        services.AddScoped<ExportExcelService, ExportExcelServiceImp>();
        services.AddScoped<Report.LAReport.Services.ExportExcelService, Report.LAReport.Services.ExportExcelServiceImp>();
        services.AddScoped<ExcelPermissionService, ExcelPermissionServiceImp>();
    }
}