using Microsoft.AspNetCore.SignalR;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.Hubs;

public interface PreMatchNotifyHub
{
    Task NotifyNewMessageIncome(int id, bool isTicketLeague, object data);

    Task NotifyNewAddRecipient(int id, bool isTicketLeague);

    Task NotifyChangeStatusIncome();

    Task NotifyUserOnClickToast(int ticketId, int eventType);
}

public class PreMatchNotifyHubImp : Hub<PreMatchNotifyHub>
{
}