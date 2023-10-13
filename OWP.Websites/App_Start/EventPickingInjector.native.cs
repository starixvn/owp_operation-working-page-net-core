using OWP.EventPicking;
using OWP.EventPicking.Middleware;
using OWP.EventPicking.Services;

namespace OWP.Websites;

public static class EventPickingInjector
{
    public static void AddEventPicking(this IServiceCollection services)
    {
        services.AddScoped<Repository, RepositoryImp>();
        services.AddScoped<BuildMainMatchMaxBet, BuildMainMatchMaxBetImp>();
        services.AddScoped<BuildChildMatchMaxBet, BuildChildMatchMaxBetImp>();
        services.AddScoped<BuildRowColor, BuildRowColorImp>();
        services.AddScoped<BuildMainAndChildMatches, BuildMainAndChildMatchesImp>();
        services.AddScoped<BuildMatchOfferByLiveSetting, BuildMatchOfferByLiveSettingImp>();
        services.AddScoped<BuildProvider, BuildProviderImp>();
        services.AddScoped<GetEventValidator>();
        services.AddScoped<LogService, LogServiceImp>();
        services.AddScoped<QueryEventPickingService, QueryEventPickingServiceImp>();
    }
}