using OWP.Application.Authorize;
using OWP.Application.DocumentTitle;
using OWP.Application.Header;
using OWP.Application.Message;
using OWP.Application.QuickSearch;

namespace OWP.Websites.App_Start;

public static class ApplicationInjector
{
    public static void AddApplicationInjector(this IServiceCollection services)
    {
        services.AddScoped<HeaderService, HeaderServiceImp>();
        services.AddScoped<AuthorizeService, AuthorizeServiceImp>();
        services.AddScoped<MessageRepository, MessageRepositoryImp>();
        services.AddScoped<DocumentTitleRepository, DocumentTitleRepositoryImp>();
        services.AddScoped<QuickSearchRepository, QuickSearchRepositoryImp>();
    }
}
