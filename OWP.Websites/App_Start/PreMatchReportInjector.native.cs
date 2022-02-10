using OWP.PreMatchReport;

namespace OWP.Websites;

public static class PreMatchReportInjector
{
    public static void AddPreMatchReportService(this IServiceCollection services)
    {
        services.AddScoped<Repository, RepositoryImp>();
    }
}