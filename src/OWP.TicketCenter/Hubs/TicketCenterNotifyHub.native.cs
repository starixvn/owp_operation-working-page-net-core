using Microsoft.AspNetCore.SignalR;
using OWP.TicketCenter.ViewModels;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.Hubs;

public interface TicketCenterNotifyHub
{
    Task NotifyNewMessageIncome(MessageDetailViewModel messageResponse);

    Task NotifyNewAddRecipient(TicketInfo ticketInfo);

    Task NotifyChangeStatusIncome();

    Task NotifyChangeSubTypeIncome(int ticketId, int ticketSubTypeId, string ticketSubTypeName);

    Task NotifyUserOnClickToast(int ticketId, int eventType);

    Task NotifyNewMessageLga(int owpMatchId, int actorId);

    Task NotifyNewAddRecipientLga(int owpMatchId, int actorId);
}

public class TicketCenterNotifyHubImp : Hub<TicketCenterNotifyHub>
{
}