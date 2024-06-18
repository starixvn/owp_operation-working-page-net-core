
using OWP.NotificationSetting;

namespace OWP.Websites.App_Start;

public static class NotificationSettingInjector
{
    public static void AddNotificationSetting(this IServiceCollection services)
    {
        services.AddScoped<Repository, RepositoryImp>();
    }
}
