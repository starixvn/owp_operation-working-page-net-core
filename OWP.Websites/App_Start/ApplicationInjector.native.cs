using OWP.Application.Authorize;
using OWP.Application.Header;

namespace OWP.Websites;

public static class ApplicationInjector
{
    public static void AddApplicationInjector(this IServiceCollection services)
    {
        services.AddScoped<HeaderService, HeaderServiceImp>();
        services.AddScoped<AuthorizeService, AuthorizeServiceImp>();
    }
}
