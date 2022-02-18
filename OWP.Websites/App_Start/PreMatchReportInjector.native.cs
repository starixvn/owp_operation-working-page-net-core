using OWP.PreMatchReport;
using OWP.PreMatchReport.Services;

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