namespace OWP.Websites.App_Start;

using OSLeagueSetting;
using OSLeagueSetting.Factory;
using OSLeagueSetting.Services;

public static class OSLeagueSettingInjectior
{
    public static void AddOSLeagueSetting(this IServiceCollection services)
    {
        services.AddScoped<Repository, RepositoryImp>();
        services.AddScoped<GetLeagueSettingService, GetLeagueSettingServiceImp>();
        services.AddScoped<AddSettingService, AddSettingServiceImp>();
        services.AddScoped<EditSettingService, EditSettingServiceImp>();
        services.AddScoped<DuplicateSettingService, DuplicateSettingServiceImp>();
        services.AddScoped<SettingRuleCreator, SettingRuleCreatorImp>();
        services.AddScoped<LogService, LogServiceImp>();
    }
}