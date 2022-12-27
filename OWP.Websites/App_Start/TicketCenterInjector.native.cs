using OWP.TicketCenter;
using OWP.TicketCenter.Middleware;
using OWP.TicketCenter.Notification.Services;
using OWP.TicketCenter.Services;

namespace OWP.Websites;

public static class TicketCenterInjector
{
    public static void AddTicketCenterService(this IServiceCollection services)
    {
        services.AddScoped<PermissionFilter>();
        services.AddScoped<PermissionOnExistedTicket>();
        services.AddScoped<PermissionOnAction>();
        services.AddScoped<PermissionOnOpenTicketDetail>();
        services.AddScoped<PermissionRemoveLastRecipient>();
        services.AddScoped<Repository, RepositoryImp>();
        services.AddScoped<RecipientService, RecipientServiceImp>();
        services.AddScoped<TicketDetailService, TicketDetailServiceImp>();

        services.AddScoped<TicketLeagueDetailService, TicketLeagueDetailServiceImp>();
        services.AddScoped<TicketMatchDetailService, TicketMatchDetailServiceImp>();
        services.AddScoped<TicketOtherDetailService, TicketOtherDetailServiceImp>();
        services.AddScoped<TicketSupportDetailService, TicketSupportDetailServiceImp>();
        services.AddScoped<TicketLiveIncidentDetailService, TicketLiveIncidentDetailServiceImp>();
        services.AddScoped<MessageService, MessageServiceImp>();
        services.AddScoped<GetEventTicketsService, GetEventTicketsServiceImp>();

        services.AddScoped<NotificationService, NotificationServiceImp>();
        services.AddScoped<PermissionService, PermissionServiceImp>();
    }
}