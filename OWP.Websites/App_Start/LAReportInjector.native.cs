namespace OWP.Websites.App_Start;

using Report.LAReport.Services;
using Report.TraderReport.Services;
using Services;
using ExportExcelService = Report.TraderReport.Services.ExportExcelService;
using ExportExcelServiceImp = Report.TraderReport.Services.ExportExcelServiceImp;

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