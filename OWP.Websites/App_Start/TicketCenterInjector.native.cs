using OWP.TicketCenter;
using OWP.TicketCenter.Middleware;
using OWP.TicketCenter.Notification;
using OWP.TicketCenter.Notification.Services;
using OWP.TicketCenter.Services;

namespace OWP.Websites.App_Start;

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
        services.AddScoped<MessageService, MessageServiceImp>();
        services.AddScoped<GetEventTicketsService, GetEventTicketsServiceImp>();
        services.AddScoped<GetInfoFeedback, GetInfoFeedbackImp>();
        services.AddScoped<NotificationService, NotificationServiceImp>();
        services.AddScoped<PreMatchNotificationService, PreMatchNotificationServiceImp>();
        services.AddScoped<PermissionService, PermissionServiceImp>();
        services.AddScoped<NotificationRepository, NotificationRepositoryImp>();
    }
}