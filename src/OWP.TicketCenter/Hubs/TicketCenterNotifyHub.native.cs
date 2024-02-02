using Microsoft.AspNetCore.SignalR;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Hubs;

public interface TicketCenterNotifyHub
{
    Task NotifyNewMessageIncome(MessageDetailViewModel messageResponse);

    Task NotifyNewAddRecipient();

    Task NotifyChangeStatusIncome();

    Task NotifyUserOnClickToast(int ticketId, int eventType);

    Task NotifyNewMessageLga(int owpMatchId, int actorId);

    Task NotifyNewAddRecipientLga(int owpMatchId, int actorId);
}

public class TicketCenterNotifyHubImp : Hub<TicketCenterNotifyHub>
{
}