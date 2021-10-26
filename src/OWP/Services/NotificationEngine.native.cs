namespace OWP.Services
{
    using System;
    using Fanex.Logging;
    using Microsoft.AspNet.SignalR;
    using Models;

    public class NotificationEngine
    {
        public static NotificationEngine Singleton { get; } = new NotificationEngine();

        public void Notify(NotificationEventArgs arg)
        {
            try
            {
                var context = GlobalHost.ConnectionManager.GetHubContext<NotifyHub>();
                if (string.IsNullOrEmpty(arg.UserId))
                {
                    return;
                }

                var userClient = context.Clients.User(arg.UserId);
                userClient.notify(new
                {
                    notificationId = Guid.NewGuid(),
                    messages = arg.Messages
                });
            }

            catch (Exception e)
            {
                Logger.Log.Info($"NotificationEngine-Notify: ${e.Message}");
            }
        }
    }
}