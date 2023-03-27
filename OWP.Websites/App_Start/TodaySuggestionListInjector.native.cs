using OWP.AssigneeSuggestionList.Services;

namespace OWP.Websites.App_Start;

public static class TodaySuggestionListInjector
{
    public static void AddSuggestionList(this IServiceCollection services)
    {
        services.AddScoped<AssignmentService, AssignmentServiceImp>();
        services.AddScoped<MJGroupService, MJGroupServiceImp>();
        services.AddScoped<OneGameService, OneGameServiceImp>();
        services.AddScoped<LinkNGameService, LinkNGameServiceImp>();
        services.AddScoped<FirstHalfService, FirstHalfServiceImp>();
        services.AddScoped<FantasyService, FantasyServiceImp>();
        services.AddScoped<CornerService, CornerServiceImp>();
        services.AddScoped<XMinsGroupService, XMinsGroupServiceImp>();
    }
}
