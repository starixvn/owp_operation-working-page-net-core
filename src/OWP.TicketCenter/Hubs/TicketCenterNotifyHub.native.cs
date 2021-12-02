using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace OWP.TicketCenter.Hubs
{
    public interface TicketCenterNotifyHub
    {
        Task NotifyNewMessageIncome();

        Task NotifyNewAddRecipient();

        Task NotifyChangeStatusIncome();

        Task NotifyUserOnClickToast(int ticketId, int eventType);
    }

    public class TicketCenterNotifyHubImp : Hub<TicketCenterNotifyHub>
    {
    }
}
