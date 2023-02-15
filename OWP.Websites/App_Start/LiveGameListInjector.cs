

namespace OWP.Websites.App_Start;

using LiveGameList;
using LiveGameList.Services;

public static class LiveGameListInjector
{
    public static void AddLiveGameListService(this IServiceCollection services)
    {
        services.AddScoped<Repository, RepositoryImp>();
        services.AddScoped<EventInformationService, EventInformationServiceImp>();
        services.AddScoped<GetAdminSiteMatchIdService, GetAdminSiteMatchIdServiceImp>();
    }

}