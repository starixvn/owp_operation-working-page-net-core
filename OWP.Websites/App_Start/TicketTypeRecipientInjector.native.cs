
using OWP.TicketTypeRecipient.Middleware;

namespace OWP.Websites;

public static class TicketTypeRecipientInjector
{
  public static void AddTicketTypeRecipientService(this IServiceCollection services)
  {
    services.AddScoped<DupplicateSubtypeValidator>();
  }
}