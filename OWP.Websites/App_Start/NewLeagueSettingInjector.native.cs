using OWP.NewLeagueSetting.Data;
namespace OWP.Websites;

public static class NewLeagueSettingInjector
{
    public static void AddNewLeagueSettingService(this IServiceCollection services)
    {

        services.AddScoped<Repository, RepositoryImp>();
    }
}