using OWP.TicketTypeRecipient.Middleware;

namespace OWP.Websites.App_Start;

public static class TicketTypeRecipientInjector
{
    public static void AddTicketTypeRecipientService(this IServiceCollection services)
    {
        services.AddScoped<DuplicateSubtypeValidator>();
    }
}