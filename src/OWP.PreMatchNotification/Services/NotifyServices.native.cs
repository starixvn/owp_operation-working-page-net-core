namespace OWP.PreMatchNotification.Services
{
    using Microsoft.AspNetCore.SignalR;
    using OWP.Hubs;
    using OWP.PreMatchNotification.Models;
  using OWP.Services;

  public interface NotifyServices
    {
        Task Notify(List<MessageModel> messages);
    }

    public class NotifyServicesImp : NotifyServices
    {
        private readonly IHubContext<ChangedInfoNotifyHubImp, ChangedInfoNotifyHub> notifyChangedHub;

        private readonly LogServiceWrapper logServiceWrapper;

        public NotifyServicesImp(IHubContext<ChangedInfoNotifyHubImp, ChangedInfoNotifyHub> notifyChangedHub, LogServiceWrapper logServiceWrapper)
        {
            this.notifyChangedHub = notifyChangedHub;
            this.logServiceWrapper = logServiceWrapper;
        }

        public async Task Notify(List<MessageModel> messages)
        {
          await logServiceWrapper.Log("Start log", messages.Count);
            foreach (var message in messages)
            {
                var result = new
                {
                    notificationId = Guid.NewGuid(),
                    message
                };
await logServiceWrapper.Log("ReceivedUserIds", message);
                await notifyChangedHub.Clients
                    .Users(message.ReceivedUserIds)
                    .notifyChangedInformationData(result);
            }
        }
    }
}