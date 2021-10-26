using Microsoft.AspNetCore.SignalR;
using OWP.Models;
using OWP.Services;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Hubs;

namespace OWP.TicketCenter.Notification.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Fanex.Data.Repository;
    using Models;
    using Utils;

    public interface NotificationService
    {
        Task Notify(string sessionId);

        Task Notify(string sessionId, int ticketId);

        Task<bool> NotifyNewMessageIncome(int ticketId, int creatorId);

    }

    public class NotificationServiceImp : NotificationService
    {
        private readonly Repository repository;

        private readonly IHubContext<NewMessageIncomeNotifyHubImp, NewMessageIncomeNotifyHub> newMessageIncomeNotify;

        public NotificationServiceImp(
            Repository repository,
            IHubContext<NewMessageIncomeNotifyHubImp, NewMessageIncomeNotifyHub> newMessageIncomeNotify)
        {
            this.repository = repository;
            this.newMessageIncomeNotify = newMessageIncomeNotify;
        }

        public async Task Notify(string sessionId)
        {
            await NotifyInternal(sessionId);
        }

        public async Task Notify(string sessionId, int ticketId)
        {
            await NotifyInternal(sessionId, ticketId);
        }

        private async Task NotifyInternal(string sessionId, int ticketId = 0)
        {
            var tuple = await QueryBySession(sessionId);
            tuple.Item1.Select(user =>
            {
                var messages = tuple.Item2
                    .Where(x => x.UserId == user.UserId)
                    .Select(x => new NotificationModel()
                {
                    UserId = x.UserId,
                    IsImportant = x.IsImportant,
                    MessageType = x.MessageType,
                    MessageContent = x.MessageContent,
                    TicketId = ticketId
                    }).ToList();

                return new NotificationEventArgs
                {
                    UserId = user.UserName,
                    Messages = messages.ConvertAll(x => (Message)x)
                };
            }).ToList().ForEach(arg =>
            {
                NotificationEngine.Singleton.Notify(arg);
            });
        }

        private async Task<ValueTuple<IList<User>, IList<Message>>> QueryBySession(string sessionId)
        {
            var criteria = new GetMessageBySessionCriteria
            {
                SessionId = sessionId
            };
            criteria.ThrowExceptionIfInvalid();
            return await repository.QueryBySession(criteria);
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
