using Microsoft.Extensions.DependencyInjection;
using OWP.TicketCenter;
using OWP.TicketCenter.Services;

namespace OWP.Websites.App_Start
{
    public static class TicketCenterInjector
    {
        public static void InjectTicketCenterService(this IServiceCollection services)
        {
            services.AddScoped<Repository, RepositoryImp>();
            services.AddScoped<InsertOrUpdateRecipientService, InsertOrUpdateRecipientServiceImp>();
            services.AddScoped<TicketDetailService, TicketDetailServiceImp>();
            services.AddScoped<TicketLeagueDetailService, TicketLeagueDetailServiceImp>();
            services.AddScoped<TicketMatchDetailService, TicketMatchDetailServiceImp>();
            services.AddScoped<TicketOtherDetailService, TicketOtherDetailServiceImp>();
            services.AddScoped<EventTicketMatchService, EventTicketMatchServiceImp>();
            services.AddScoped<EventTicketOtherService, EventTicketOtherServiceImp>();
            services.AddScoped<EventTicketLeagueService, EventTicketLeagueServiceImp>();
        }
    }
}
