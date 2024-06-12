using OWP.ResourceManagement;

namespace OWP.Websites.App_Start;

public static class ResourceManagementInjector
{
    public static void AddResourceManagement(this IServiceCollection services)
    {
        services.AddScoped<Repository, RepositoryImp>();
    }
}
