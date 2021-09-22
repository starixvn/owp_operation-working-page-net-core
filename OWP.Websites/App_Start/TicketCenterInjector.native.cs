using Microsoft.Extensions.DependencyInjection;
using OWP.TicketCenter;

namespace OWP.Websites.App_Start
{
    public static class TicketCenterInjector
    {
        public static void InjectTicketCenterService(this IServiceCollection services)
        {
            services.AddScoped<Repository, RepositoryImp>();
        }
    }
}
