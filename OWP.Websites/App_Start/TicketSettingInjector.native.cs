using OWP.TicketSetting.RecipientGroup.Services;
using OWP.TicketSetting.TicketEvent;
using OWP.TicketSetting.TicketTypeRecipient.Middleware;

namespace OWP.Websites;

public static class TicketSettingInjector
{
    public static void AddTicKetEventService(this IServiceCollection services)
    {
        services.AddScoped<Repository, RepositoryImp>();
    }

    public static void AddRecipientGroup(this IServiceCollection services)
    {
        services.AddScoped<CommandRepository, CommandRepositoryImp>();
    }

    public static void AddTicketTypeRecipientService(this IServiceCollection services)
    {
        services.AddScoped<DuplicateSubtypeValidator>();
    }
}
