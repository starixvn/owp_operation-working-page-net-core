using OWP.AccountSetting.Profile;

namespace OWP.Websites;

public static class AccountSettingInjector
{
    public static void AddAccountSettingService(this IServiceCollection services)
    {
        services.AddScoped<ProfileRepository, ProfileRepositoryImp>();
    }
}