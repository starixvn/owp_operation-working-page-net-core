namespace OWP.PreMatchNotification.Services
{
    using Microsoft.AspNetCore.SignalR;
    using OWP.Hubs;
    using OWP.PreMatchNotification.Models;

    public interface NotifyServices
    {
        Task Notify(List<MessageModel> messages);
    }

    public class NotifyServicesImp : NotifyServices
    {
        private readonly IHubContext<ChangedInfoNotifyHubImp, ChangedInfoNotifyHub> notifyChangedHub;

        public NotifyServicesImp(IHubContext<ChangedInfoNotifyHubImp, ChangedInfoNotifyHub> notifyChangedHub)
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
                    .notifyChangedInformationData(result);
            }
        }
    }
}