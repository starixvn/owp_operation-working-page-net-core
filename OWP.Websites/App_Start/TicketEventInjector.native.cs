using OWP.TicketEvent;

namespace OWP.Websites.App_Start;

public static class TicketEventInjector
{
    public static void AddTicKetEventService(this IServiceCollection services)
    {
        services.AddScoped<Repository, RepositoryImp>();
    }
}
