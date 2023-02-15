namespace OWP.Websites;

using LeagueManagement;
using LeagueManagement.Service;

public static class LeagueManagementInjector
{
    public static void AddLeagueManagementService(this IServiceCollection services)
    {
        services.AddScoped<LeagueManagementService, LeagueManagementServiceImp>();
        services.AddScoped<SpecialProductService, SpecialProductServiceImp>();
        services.AddScoped<Repository, RepositoryImp>();
    }
}