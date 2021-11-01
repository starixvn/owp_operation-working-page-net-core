using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace OWP.TicketCenter.Hubs
{
    public interface TicketCenterNotifyHub
    {
        Task NotifyNewMessageIncome();

        Task NotifyNewAddRecipient();
    }

    public class TicketCenterNotifyHubImp: Hub<TicketCenterNotifyHub>
    {
    }
}
