using OWP.ResourceManagement;

namespace OWP.Websites;

public static class ResourceManagementInjector
{
    public static void AddResourceManagement(this IServiceCollection services)
    {
        services.AddScoped<Repository, RepositoryImp>();
    }
}
