namespace OWP.Websites.App_Start;

using OSLeagueSetting;
using OWP.OSLeagueSetting.Services;

public static class OSLeagueSettingInjectior
{
    public static void AddOSLeagueSetting(this IServiceCollection services)
    {
        services.AddScoped<Repository, RepositoryImp>();
        services.AddScoped<GetLeagueSettingService, GetLeagueSettingServiceImp>();
        services.AddScoped<AddSettingService, AddSettingServiceImp>();
        services.AddScoped<EditSettingService, EditSettingServiceImp>();
    }
}