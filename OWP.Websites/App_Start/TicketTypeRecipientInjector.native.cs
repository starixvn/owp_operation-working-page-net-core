
namespace OWP.Websites;

using TicketTypeRecipient.Middleware;

public static class TicketTypeRecipientInjector
{
  public static void AddTicketTypeRecipientService(this IServiceCollection services)
  {
    services.AddScoped<DuplicateSubtypeValidator>();
  }
}