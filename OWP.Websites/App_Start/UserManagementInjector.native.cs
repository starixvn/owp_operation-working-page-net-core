using OWP.UserManagement;
using OWP.UserManagement.Middleware;
using OWP.UserManagement.Services;

namespace OWP.Websites;

public static class UserManagementInjector
{
    public static void AddUserManagement(this IServiceCollection services)
    {
        services.AddScoped<Repository, RepositoryImp>();
        services.AddScoped<ExportService, ExportServiceImp>();
        services.AddScoped<ModelService, ModelServiceImp>();
        services.AddScoped<UsersService, UsersServiceImp>();
        services.AddScoped<GetUserValidator>();
    }
}