using Microsoft.AspNetCore.SignalR;

namespace OWP.TicketCenter.Hubs;

public interface TicketCenterNotifyHub
{
    Task NotifyNewMessageIncome();

    Task NotifyNewAddRecipient();

    Task NotifyChangeStatusIncome();

    Task NotifyUserOnClickToast(int ticketId, int eventType);

    Task NotifyNewMessageLga(int owpMatchId, int actorId);

    Task NotifyNewAddRecipientLga(int owpMatchId, int actorId);

}

public class TicketCenterNotifyHubImp : Hub<TicketCenterNotifyHub>
{
}