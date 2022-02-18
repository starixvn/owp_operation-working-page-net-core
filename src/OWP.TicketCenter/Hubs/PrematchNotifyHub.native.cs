using Microsoft.AspNetCore.SignalR;


namespace OWP.TicketCenter.Hubs
{
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
}
