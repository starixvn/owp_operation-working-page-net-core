using OWP.Application.Authorize;
using OWP.Application.Header;
using OWP.Application.Message;

namespace OWP.Websites;

public static class ApplicationInjector
{
    public static void AddApplicationInjector(this IServiceCollection services)
    {
        services.AddScoped<HeaderService, HeaderServiceImp>();
        services.AddScoped<AuthorizeService, AuthorizeServiceImp>();
        services.AddScoped<MessageRepository, MessageRepositoryImp>();
    }
}
