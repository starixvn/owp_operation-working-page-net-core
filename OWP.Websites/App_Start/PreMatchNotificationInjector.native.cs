namespace OWP.Websites.App_Start
{
    using Microsoft.Extensions.DependencyInjection;
    using PreMatchNotification;
    using PreMatchNotification.Services;

    public static class PreMatchNotificationInjector
    {
        public static void InjectPreMatchNotification(this IServiceCollection services)
        {
            services.AddScoped<Repository, RepositoryImp>();
            services.AddScoped<CacheManipulator, CacheManipulatorImp>();
            services.AddScoped<ChangedValueByCacheAndDatabase, ChangedValueByCacheAndDatabaseImp>();
        }
    }
}