using OWP.Report.PreMatchReport;
using OWP.Report.PreMatchReport.Services;

namespace OWP.Websites;

public static class PreMatchReportInjector
{
    public static void AddPreMatchReportService(this IServiceCollection services)
    {
        services.AddScoped<Repository, RepositoryImp>();
        services.AddScoped<PreMatchReportExportService, PreMatchReportExportServiceImp>();
        services.AddScoped<GetPreMatchReportService, GetPreMatchReportServiceImp>();
    }
}