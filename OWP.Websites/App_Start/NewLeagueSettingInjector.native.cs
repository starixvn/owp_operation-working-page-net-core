using OWP.NewLeagueSetting.Data;
using OWP.NewLeagueSetting.Service;

namespace OWP.Websites;

public static class NewLeagueSettingInjector
{
    public static void AddNewLeagueSettingService(this IServiceCollection services)
    {

        services.AddScoped<Repository, RepositoryImp>();
        services.AddScoped<DuplicateSettingService, DuplicateSettingServiceImp>();
    }
}