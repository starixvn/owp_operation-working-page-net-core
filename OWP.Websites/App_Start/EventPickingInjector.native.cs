using OWP.EventPicking;
using OWP.EventPicking.Api.Middleware;
using OWP.EventPicking.Api.Services;

namespace OWP.Websites;

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
        services.AddScoped<PullMainLeagueService, PullMainLeagueServiceImp>();
        services.AddScoped<BuildMBForClubFriendlyMajorLeagueBuilder, BuildMBForClubFriendlyMajorLeagueBuilderImp>();
        services.AddScoped<MapMainLeagueForEventService, MapMainLeagueForEventServiceImp>();
        services.AddScoped<MappingRuleSyncMBLeagueFriendlyService, MappingRuleSyncMBLeagueFriendlyServiceImp>();
        services.AddScoped<CacheManipulator, CacheManipulatorImp>();
        services.AddScoped<BuildMaxbetSpreadChildMatch, BuildMaxbetSpreadChildMatchImp>();
    }
}