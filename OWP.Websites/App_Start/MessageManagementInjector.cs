using OWP.MessageManagement;

namespace OWP.Websites;

public static class MessageManagementInjector
{
    public static void AddMessageManagementService(this IServiceCollection services)
    {

        services.AddScoped<Repository, RepositoryImp>();
    }
}
