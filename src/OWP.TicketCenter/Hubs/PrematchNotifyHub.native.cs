using Microsoft.AspNetCore.SignalR;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Hubs;

public interface PreMatchNotifyHub
{
    Task NotifyNewMessageIncome(int id, bool isTicketLeague, object data);

    Task NotifyChangeStatusIncome(int ticketId, string ticketStatus);

    Task NotifyChangeSybTypeIncome(TicketInfo ticketStatus);

    Task NotifyUserOnClickToast(int ticketId, int eventType);

    Task NotifyNewAddRecipient(int id, bool isTicketLeague);

    Task NotifyUpdateReactIconMessage(MessageReactIcon messageReactIcon, string action, int ticketId);
}

public class PreMatchNotifyHubImp : Hub<PreMatchNotifyHub>;