using OWP.AssigneeSuggestionList;
using OWP.AssigneeSuggestionList.Services;
using OWP.AssigneeSuggestionList.Services.RealMatch;

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
        services.AddScoped<SpecialGroupService, SpeacialGroupServiceImp>();
        services.AddScoped<SabaMatchServices, SabaMatchesServicesImp>();
        services.AddScoped<RealMatchServices, RealMatchServicesImp>();
        services.AddScoped<WorkingShiftService, WorkingShiftServiceImp>();
        services.AddScoped<EtPkService, EtPkServiceImp>();
        services.AddScoped<MainGameService, MainGameServiceImp>();
        services.AddScoped<SpecialGameService, SpecialGameServiceImp>();
        services.AddScoped<LinkGameService, LinkGameServiceImp>();
        services.AddScoped<MatchInfoServices, MatchInfoServicesImp>();
        services.AddScoped<SingleTeamService, SingleTeamServiceImp>();
        services.AddScoped<WinnerService, WinnerServiceImp>();
        services.AddScoped<BookingService, BookingServiceImp>();
        services.AddScoped<SpecialGameGroupService, SpecialGameGroupServiceImp>();
        // categorize
        services.AddScoped<CategorizeRealMatchServices, CategorizeRealMatchServicesImp>();
        services.AddScoped<CategorizeSabaMatchServices, CategorizeSabaMatchServicesImp>();
    }
}
