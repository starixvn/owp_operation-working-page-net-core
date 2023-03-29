using OWP.AssigneeSuggestionList;
using OWP.AssigneeSuggestionList.Services;

namespace OWP.Websites;

public static class AssigneeSuggestionListInjector
{
    public static void AddSuggestionList(this IServiceCollection services)
    {
        services.AddScoped<Repository, RepositoryImp>();
        services.AddScoped<MJGroupService, MJGroupServiceImp>();
        services.AddScoped<OneGameService, OneGameServiceImp>();
        services.AddScoped<LinkNGameService, LinkNGameServiceImp>();
        services.AddScoped<FirstHalfService, FirstHalfServiceImp>();
        services.AddScoped<FantasyService, FantasyServiceImp>();
        services.AddScoped<CornerService, CornerServiceImp>();
        services.AddScoped<XMinsGroupService, XMinsGroupServiceImp>();
        services.AddScoped<WorkingScheduleService, WorkingScheduleServiceImp>();
        services.AddScoped<SingleGameGroupService, SingleGameGroupServiceImp>();
        services.AddScoped<SabaMatchesServices, SabaMatchesServicesImp>();
    }
}
