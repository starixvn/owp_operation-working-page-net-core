namespace OWP.Websites;

using TicketCenter;
using TicketCenter.Middleware;
using TicketCenter.Notification.Services;
using TicketCenter.Services;

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

        services.AddScoped<NotificationService, NotificationServiceImp>();
        services.AddScoped<PermissionService, PermissionServiceImp>();
    }
}