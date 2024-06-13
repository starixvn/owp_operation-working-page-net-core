namespace OWP.Websites.App_Start;
using MessageManagement;

public static class MessageManagementInjector
{
    public static void AddMessageManagementService(this IServiceCollection services)
    {

        services.AddScoped<Repository, RepositoryImp>();
    }
}
