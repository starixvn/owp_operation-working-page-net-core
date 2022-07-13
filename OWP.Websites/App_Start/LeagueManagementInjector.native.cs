using OWP.LeagueManagement;
using OWP.LeagueManagement.Service;

namespace OWP.Websites;

public static class LeagueManagementInjector
{
    public static void AddLeagueManagementService(this IServiceCollection services)
    {
        services.AddScoped<LeagueManagementService, LeagueManagementServiceImp>();
        services.AddScoped<SpecialProductService, SpecialProductServiceImp>();
    }
}