
using OWP.NotificationSetting;

namespace OWP.Websites;

public static class NotificationSettingInjector
{
    public static void AddNotificationSetting(this IServiceCollection services)
    {
        services.AddScoped<Repository, RepositoryImp>();
    }
}
