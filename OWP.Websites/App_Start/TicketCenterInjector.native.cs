using Microsoft.Extensions.DependencyInjection;
using OWP.TicketCenter;
using OWP.TicketCenter.Middleware;
using OWP.TicketCenter.Notification.Services;
using OWP.TicketCenter.Services;

namespace OWP.Websites.App_Start
{
    public static class TicketCenterInjector
    {
        public static void InjectTicketCenterService(this IServiceCollection services)
        {
            services.AddScoped<PermissionFilter>();
            services.AddScoped<Repository, RepositoryImp>();
            services.AddScoped<RecipientService, RecipientServiceImp>();
            services.AddScoped<TicketDetailService, TicketDetailServiceImp>();
            services.AddScoped<TicketLeagueDetailService, TicketLeagueDetailServiceImp>();
            services.AddScoped<TicketMatchDetailService, TicketMatchDetailServiceImp>();
            services.AddScoped<TicketOtherDetailService, TicketOtherDetailServiceImp>();
            services.AddScoped<EventTicketMatchService, EventTicketMatchServiceImp>();
            services.AddScoped<EventTicketOtherService, EventTicketOtherServiceImp>();
            services.AddScoped<EventTicketLeagueService, EventTicketLeagueServiceImp>();
            services.AddScoped<NewTicketLeagueMessageDBWriter, NewTicketLeagueMessageDBWriterImp>();
            services.AddScoped<NewTicketMatchMessageDBWriter, NewTicketMatchMessageDBWriterImp>();
            services.AddScoped<NewTicketOtherMessageDBWriter, NewTicketOtherMessageDBWriterImp>();
            services.AddScoped<NotificationService, NotificationServiceImp>();
            services.AddScoped<PermissionService, PermissionServiceImp>();
            services.AddScoped<NewRecipientMessageDBWriter, NewRecipientMessageDBWriterImp>();
        }
    }
}