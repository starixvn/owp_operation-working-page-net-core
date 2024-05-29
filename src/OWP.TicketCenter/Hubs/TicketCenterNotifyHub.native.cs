using Microsoft.AspNetCore.SignalR;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Hubs;

public interface TicketCenterNotifyHub
{
    Task NotifyNewMessageIncome(MessageDetailViewModel messageResponse, TicketInfo ticketInfo);

    Task NotifyNewAddRecipient(TicketInfo ticketInfo);

    Task NotifyChangeStatusIncome(TicketInfo ticketInfo);

    Task NotifyChangeSubTypeIncome(TicketInfo ticketInfo);

    Task NotifyUserOnClickToast(int ticketId, int eventType);

    Task NotifyNewMessageLga(int owpMatchId, int actorId);

    Task NotifyNewAddRecipientLga(int owpMatchId, int actorId);

    Task NotifyUpdateReactIconMessage(MessageReactIcon messageReactIcon, string action, int ticketId);
}

public class TicketCenterNotifyHubImp : Hub<TicketCenterNotifyHub>
{
}