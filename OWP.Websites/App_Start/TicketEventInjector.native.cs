namespace OWP.Websites;

using TicketEvent;

public static class TicketEventInjector
{
    public static void AddTicKetEventService(this IServiceCollection services)
    {
        services.AddScoped<Repository, RepositoryImp>();
    }
}
