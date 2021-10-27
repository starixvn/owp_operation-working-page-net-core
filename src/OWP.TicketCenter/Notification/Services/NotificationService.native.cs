

using OWP.TicketCenter.Enum;

namespace OWP.TicketCenter.Notification.Services
{
    using Microsoft.AspNetCore.SignalR;
    using OWP.Services;
    using OWP.TicketCenter.Hubs;
    using System.Linq;
    using System.Threading.Tasks;
    using Models;

    public interface NotificationService
    {

        Task Notify(string sessionId, int ticketId, EventType eventType);

        Task<bool> NotifyNewMessageIncome(int ticketId, int creatorId);

    }

    public class NotificationServiceImp : NotificationService
    {
        private readonly NotifyEngine notifyEngine;

        private readonly IHubContext<NewMessageIncomeNotifyHubImp, NewMessageIncomeNotifyHub> newMessageIncomeNotify;

        private readonly Repository repository;

        public NotificationServiceImp(
            NotifyEngine notifyEngine,
            Repository repository,
            IHubContext<NewMessageIncomeNotifyHubImp, NewMessageIncomeNotifyHub> newMessageIncomeNotify)
        {
            this.notifyEngine = notifyEngine;
            this.newMessageIncomeNotify = newMessageIncomeNotify;
            this.repository = repository;
        }

        public async Task Notify(string sessionId, int ticketId, EventType eventType)
        {
            await notifyEngine.Notify(sessionId, messageTuple =>
                messageTuple.Item2.Select(x => new NotificationModel
                {
                    UserId = x.UserId,
                    IsImportant = x.IsImportant,
                    MessageType = x.MessageType,
                    MessageContent = x.MessageContent,
                    TicketId = ticketId,
                    EventType = (int)eventType
                }).ToList());
        }

        public async Task<bool> NotifyNewMessageIncome(int ticketId, int creatorId)
        {
            var userIds = await repository.GetUserIdsIgnoredCreatorId(ticketId, creatorId);
            var userIdsString = userIds.Select(u => u.ToString());
            await newMessageIncomeNotify.Clients
                .Users(userIdsString)
                .notifyNewMessageIncome();
            return await Task.FromResult(true);
        }

    }
}
