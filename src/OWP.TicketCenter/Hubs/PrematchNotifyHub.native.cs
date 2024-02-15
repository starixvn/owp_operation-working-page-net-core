using Microsoft.AspNetCore.SignalR;

namespace OWP.TicketCenter.Hubs;

public interface PreMatchNotifyHub
{
    Task NotifyNewMessageIncome(int id, bool isTicketLeague, object data);

    Task NotifyChangeStatusIncome();

    Task NotifyUserOnClickToast(int ticketId, int eventType);
}

public class PreMatchNotifyHubImp : Hub<PreMatchNotifyHub>
{
}