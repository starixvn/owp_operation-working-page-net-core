using OWP.EventPicking;
using OWP.EventPicking.Api.Middleware;
using OWP.EventPicking.Api.Services;

namespace OWP.Websites.App_Start;

public static class EventPickingInjector
{
    public static void AddEventPicking(this IServiceCollection services)
    {
        services.AddScoped<Repository, RepositoryImp>();
        services.AddScoped<BuildChildMatchMaxBet, BuildChildMatchMaxBetImp>();
        services.AddScoped<BuildRowColor, BuildRowColorImp>();
        services.AddScoped<BuildMainAndChildMatches, BuildMainAndChildMatchesImp>();
        services.AddScoped<BuildMatchOfferByLiveSetting, BuildMatchOfferByLiveSettingImp>();
        services.AddScoped<BuildProvider, BuildProviderImp>();
        services.AddScoped<GetEventValidator>();
        services.AddScoped<LogService, LogServiceImp>();
        services.AddScoped<QueryEventPickingService, QueryEventPickingServiceImp>();
        services.AddScoped<ExportService, ExportServiceImp>();
        services.AddScoped<TGRSService, TGRSServiceImp>();
    }
}