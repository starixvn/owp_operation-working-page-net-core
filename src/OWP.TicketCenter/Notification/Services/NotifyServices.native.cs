namespace OWP.TicketCenter.Notification.Services
{
    using Hubs;
    using Microsoft.AspNetCore.SignalR;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface NotifyServices
    {
        Task NotifyNewMessageIncome(IList<string> recipientIds);
    }

    public class NotifyServicesImp : NotifyServices
    {
        private readonly IHubContext<NewMessageIncomeNotifyHubImp, NewMessageIncomeNotifyHub> notifyChangedHub;

        public NotifyServicesImp(IHubContext<NewMessageIncomeNotifyHubImp, NewMessageIncomeNotifyHub> notifyChangedHub)
        {
            this.notifyChangedHub = notifyChangedHub;
        }

        public async Task NotifyNewMessageIncome(IList<string> recipientIds)
        => await notifyChangedHub.Clients
            .Users(recipientIds)
            .notifyNewMessageIncome();
    }
}