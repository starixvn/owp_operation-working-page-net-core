using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace OWP.TicketCenter.Hubs
{
    public interface NewMessageIncomeNotifyHub
    {
        Task notifyNewMessageIncome();
    }

    public class NewMessageIncomeNotifyHubImp: Hub<NewMessageIncomeNotifyHub>
    {
    }
}
