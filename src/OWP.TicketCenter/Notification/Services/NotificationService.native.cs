namespace OWP.TicketCenter.Notification.Services
{
    using Microsoft.AspNetCore.SignalR;
    using OWP.Services;
    using Hubs;
    using System.Linq;
    using System.Threading.Tasks;
    using Models;

    public interface NotificationService
    {
        Task Notify(string sessionId, int ticketId, int eventType);

        Task<bool> NotifyNewMessageIncome(int ticketId, int creatorId);

        Task<bool> NotifyUserClickOnToast(int userId, int ticketId, int eventType);

    }

    public class NotificationServiceImp : NotificationService
    {
        private readonly NotifyEngine notifyEngine;

        private readonly IHubContext<TicketCenterNotifyHubImp, TicketCenterNotifyHub> notify;

        private readonly Repository repository;

        public NotificationServiceImp(
            NotifyEngine notifyEngine,
            Repository repository,
            IHubContext<TicketCenterNotifyHubImp, TicketCenterNotifyHub> notify)
        {
            this.notifyEngine = notifyEngine;
            this.notify = notify;
            this.repository = repository;
        }

        public async Task Notify(string sessionId, int ticketId, int eventType)
        {
            await notifyEngine.Notify(sessionId, messageTuple =>
                messageTuple.Item2.Select(x => new NotificationModel
                {
                    UserId = x.UserId,
                    IsImportant = x.IsImportant,
                    MessageType = x.MessageType,
                    MessageContent = x.MessageContent,
                    TicketId = ticketId,
                    EventType = eventType
                }).ToList());
        }

        public async Task<bool> NotifyNewMessageIncome(int ticketId, int creatorId)
        {
            var userIds = await repository.GetUserIdsIgnoredCreatorId(ticketId, creatorId);
            var userIdsString = userIds.Select(u => u.ToString());
            await notify.Clients
                .Users(userIdsString)
                .NotifyNewMessageIncome();
            return await Task.FromResult(true);
        }

        public async Task<bool> NotifyUserClickOnToast(int userId, int ticketId, int eventType)
        {
            await notify.Clients
                .User(userId.ToString())
                .NotifyUserOnClickToast(ticketId, eventType);
            return await Task.FromResult(true);
        }

    }
}
