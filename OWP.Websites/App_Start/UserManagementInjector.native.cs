using OWP.UserManagement;
using OWP.UserManagement.Middleware;
using OWP.UserManagement.Services;

namespace OWP.Websites.App_Start;

public static class UserManagementInjector
{
    public static void AddUserManagement(this IServiceCollection services)
    {
        services.AddScoped<Repository, RepositoryImp>();
        services.AddScoped<ExportService, ExportServiceImp>();
        services.AddScoped<UserManagement.Services.ModelService, UserManagement.Services.ModelServiceImp>();
        services.AddScoped<UsersService, UsersServiceImp>();
        services.AddScoped<GetUserValidator>();
    }
}