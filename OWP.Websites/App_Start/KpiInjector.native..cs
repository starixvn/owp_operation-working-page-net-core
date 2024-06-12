using OWP.AccountSetting.Profile;
using OWP.Kpi.LA.Services;

namespace OWP.Websites.App_Start;

public static class KpiInjector
{
    public static void AddKpiService(this IServiceCollection services)
    {
        services.AddScoped<Repository, RepositoryImp>();
        services.AddScoped<KpiService, KpiServiceImp>();
    }
}