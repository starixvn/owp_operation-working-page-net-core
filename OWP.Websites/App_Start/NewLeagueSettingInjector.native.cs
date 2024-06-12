using OWP.NewLeagueSetting.Data;
namespace OWP.Websites.App_Start;

public static class NewLeagueSettingInjector
{
    public static void AddNewLeagueSettingService(this IServiceCollection services)
    {

        services.AddScoped<Repository, RepositoryImp>();
    }
}