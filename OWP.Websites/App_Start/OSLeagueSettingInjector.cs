using OWP.OSLeagueSetting;
using OWP.OSLeagueSetting.Factory;
using OWP.OSLeagueSetting.Services;

namespace OWP.Websites.App_Start;

public static class OSLeagueSettingInjector
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
        services.AddScoped<ExportLeagueSettingService, ExportLeagueSettingServiceImp>();
    }
}