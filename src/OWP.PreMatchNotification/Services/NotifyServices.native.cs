namespace OWP.PreMatchNotification.Services
{
  using Fanex.Logging;
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

        private readonly ILogger logger;

        public NotifyServicesImp(IHubContext<ChangedInfoNotifyHubImp, ChangedInfoNotifyHub> notifyChangedHub, ILogger logger)
        {
            this.notifyChangedHub = notifyChangedHub;
            this.logger = logger;
        }

        public async Task Notify(List<MessageModel> messages)
        {
          logger.Info("Notify:=> START");
            foreach (var message in messages)
            {
              message.MessageContent= "Starix 58 test";
                var result = new
                {
                    notificationId = Guid.NewGuid(),
                    message
                };
                logger.Info($"Notify:=>{result.message}-{result.notificationId}-{string.Join(",",message.ReceivedUserIds)}");
                /* await notifyChangedHub.Clients
                    .Users(message.ReceivedUserIds)
                    .notifyChangedInformationData(result); */
            }
          logger.Info("Notify:=> END");
        }
    }
}