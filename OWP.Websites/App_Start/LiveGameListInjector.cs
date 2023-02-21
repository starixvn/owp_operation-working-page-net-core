

using OWP.LiveGameList;
using OWP.LiveGameList.Services;

namespace OWP.Websites;

public static class LiveGameListInjector
{
    public static void AddLiveGameListService(this IServiceCollection services)
    {
        services.AddScoped<Repository, RepositoryImp>();
        services.AddScoped<EventInformationService, EventInformationServiceImp>();
        services.AddScoped<GetAdminSiteMatchIdService, GetAdminSiteMatchIdServiceImp>();
    }

}