namespace OWP.Websites;

using Report.PreMatchReport;
using Report.PreMatchReport.Services;

public static class PreMatchReportInjector
{
    public static void AddPreMatchReportService(this IServiceCollection services)
    {
        services.AddScoped<Repository, RepositoryImp>();
        services.AddScoped<PreMatchReportExportService, PreMatchReportExportServiceImp>();
        services.AddScoped<GetPreMatchReportService, GetPreMatchReportServiceImp>();
    }
}