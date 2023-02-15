namespace OWP.TicketCenter.Hubs;

using Microsoft.AspNetCore.SignalR;

public interface PreMatchNotifyHub
{
    Task NotifyNewMessageIncome();

    Task NotifyNewAddRecipient();

    Task NotifyChangeStatusIncome();

    Task NotifyUserOnClickToast(int ticketId, int eventType);
}

public class PreMatchNotifyHubImp : Hub<PreMatchNotifyHub>
{
}