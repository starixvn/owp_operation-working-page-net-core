using OWP.TeamManagement;

namespace OWP.Websites;

public static class TeamManagementInjector
{
    public static void AddTeamManagementService(this IServiceCollection services)
    {
        services.AddScoped<TeamManagementRepository, TeamManagementRepositoryImp>();
    }
}
