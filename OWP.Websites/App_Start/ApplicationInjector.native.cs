using OWP.Application.Authorize;

namespace OWP.Websites;

public static class ApplicationInjector
{
    public static void AddApplicationInjector(this IServiceCollection services)
    {
        services.AddScoped<Service, ServiceImp>();
    }
}
