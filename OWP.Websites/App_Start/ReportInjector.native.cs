using OWP.Services;
using LAReport = OWP.Report.LAReport.Services;
using MatchCountLiveReport = OWP.Report.MatchCountLiveReport;
using OvertimeReport = OWP.Report.OvertimeReport.Services;
using PreMatchReport = OWP.Report.PreMatchReport;
using TraderReport = OWP.Report.TraderReport.Services;

namespace OWP.Websites.App_Start;

public static class ReportInjector
{
    public static void AddReportService(this IServiceCollection services)
    {
        services.AddScoped<RichHttpClient, RichHttpClientImp>();
        services.AddScoped<PermissionService, PermissionServiceImp>();

        services.AddLAReportService();
        services.AddTraderReportService();
        services.AddOvertimeReport();
        services.AddPreMatchReportService();
        services.AddMatchLiveReportService();
    }

    private static void AddLAReportService(this IServiceCollection services)
    {
        services.AddScoped<LAReport.ExportExcelService, LAReport.ExportExcelServiceImp>();
        services.AddScoped<LAReport.QueryRepository, LAReport.QueryRepositoryImp>();
    }

    private static void AddTraderReportService(this IServiceCollection services)
    {
        services.AddScoped<TraderReport.RepositoryService, TraderReport.RepositoryServiceImp>();
        services.AddScoped<TraderReport.TGRSReportService, TraderReport.TGRSReportServiceImp>();
        services.AddScoped<TraderReport.ReportPermissionService, TraderReport.ReportPermissionServiceImp>();
        services.AddScoped<TraderReport.ExportExcelService, TraderReport.ExportExcelServiceImp>();
        services.AddScoped<TraderReport.ExcelPermissionService, TraderReport.ExcelPermissionServiceImp>();
    }

    private static void AddOvertimeReport(this IServiceCollection services)
    {
        services.AddScoped<OvertimeReport.CommandRepository, OvertimeReport.CommandRepositoryImp>();
        services.AddScoped<OvertimeReport.ReportByMonthServices, OvertimeReport.ReportByMonthServicesImp>();
        services.AddScoped<OvertimeReport.ExportReportByMonthServices, OvertimeReport.ExportReportByMonthServicesImp>();
    }

    private static void AddMatchLiveReportService(this IServiceCollection services)
    {
        services.AddScoped<MatchCountLiveReport.Repository, MatchCountLiveReport.RepositoryImp>();
    }

    private static void AddPreMatchReportService(this IServiceCollection services)
    {
        services.AddScoped<PreMatchReport.Repository, PreMatchReport.RepositoryImp>();
        services.AddScoped<PreMatchReport.Services.PreMatchReportExportService, PreMatchReport.Services.PreMatchReportExportServiceImp>();
        services.AddScoped<PreMatchReport.Services.GetPreMatchReportService, PreMatchReport.Services.GetPreMatchReportServiceImp>();
    }
}