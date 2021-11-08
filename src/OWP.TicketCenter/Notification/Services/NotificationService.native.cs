using System.Collections.Generic;
using OWP.Models;

namespace OWP.TicketCenter.Notification.Services
{
    using Hubs;
    using Microsoft.AspNetCore.SignalR;
    using Models;
    using OWP.Services;
    using OWP.Utils;
    using System.Linq;
    using System.Threading.Tasks;

    public interface NotificationService
    {
        Task Notify(string sessionId, int ticketId, int eventType);

        Task<bool> NotifyNewMessageIncome(int ticketId);

        Task<bool> NotifyNewAddRecipient(int ticketId);

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
            {
                return messageTuple.Item2.Select(x => new NotificationModel
                {
                    IsImportant = x.IsImportant,
                    MessageType = x.MessageType,
                    MessageContent = x.MessageContent,
                    TicketId = ticketId,
                    EventType = eventType
                }).ToList().GetRange(0, 1);
            });
        }

        public async Task<bool> NotifyNewMessageIncome(int ticketId)
        {
            var userIds = await repository.GetUserIdsByTicketId(ticketId);
            var userIdsString = userIds.Select(u => u.ToString());
            await notify.Clients
                .Users(userIdsString)
                .NotifyNewMessageIncome();
            return await Task.FromResult(true);
        }

        public async Task<bool> NotifyNewAddRecipient(int ticketId)
        {
            var userIds = await repository.GetUserIdsByTicketId(ticketId);
            var userIdsString = userIds.Select(u => u.ToString());
            await notify.Clients
                .Users(userIdsString)
                .NotifyNewAddRecipient();
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