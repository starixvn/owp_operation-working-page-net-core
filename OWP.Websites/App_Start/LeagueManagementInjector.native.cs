using OWP.LeagueManagement;
using OWP.LeagueManagement.Services;

namespace OWP.Websites.App_Start;

public static class LeagueManagementInjector
{
    public static void AddLeagueManagementService(this IServiceCollection services)
    {
        services.AddScoped<LeagueManagementService, LeagueManagementServiceImp>();
        services.AddScoped<SpecialProductService, SpecialProductServiceImp>();
        services.AddScoped<Repository, RepositoryImp>();
    }
}