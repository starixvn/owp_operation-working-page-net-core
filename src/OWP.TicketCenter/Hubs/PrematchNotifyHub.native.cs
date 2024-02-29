using Microsoft.AspNetCore.SignalR;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.Hubs;

public interface PreMatchNotifyHub
{
    Task NotifyNewMessageIncome(int id, bool isTicketLeague, object data);

    Task NotifyChangeStatusIncome(int ticketId, string ticketStatus);

    Task NotifyChangeSybTypeIncome(TicketInfo ticketStatus);

    Task NotifyUserOnClickToast(int ticketId, int eventType);

    Task NotifyNewAddRecipient(int id, bool isTicketLeague);
}

public class PreMatchNotifyHubImp : Hub<PreMatchNotifyHub>
{
}