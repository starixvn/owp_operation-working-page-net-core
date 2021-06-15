namespace OWP.PreMatchNotification.Services
{
    using Microsoft.AspNetCore.SignalR;
    using OWP.PreMatchNotification.Hubs;
    using OWP.PreMatchNotification.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface NotifyServices
    {
        Task Notify(List<MessageModel> messages);
    }

    public class NotifyServicesImp: NotifyServices
    {
        private readonly IHubContext<PreMatchInfoNotifyHubImp, PreMatchInfoNotifyHub> notifyChangedHub;

        public NotifyServicesImp(IHubContext<PreMatchInfoNotifyHubImp, PreMatchInfoNotifyHub> notifyChangedHub)
        {
            this.notifyChangedHub = notifyChangedHub;
        }

        public async Task Notify(List<MessageModel> messages)
        {
            foreach (var message in messages)
            {
                var result = new
                {
                    notificationId = Guid.NewGuid(),
                    message
                };

                await notifyChangedHub.Clients
                    .Users(message.ReceivedUserIds)
                    .notifyChangedInfomationData(result);
            }
        }
    }
}
