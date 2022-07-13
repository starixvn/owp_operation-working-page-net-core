namespace OWP.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Criteria;
    using Fanex.Data.Repository;
    using Models;
    using OWP.Extensions;

    public interface NotificationService
    {
        Task Notify(string sessionId);

        Task Notify(string sessionId, int preMatchIssueId);

        Task Notify();
    }

    public class NotificationServiceImp : NotificationService
    {
        private readonly IDynamicRepository repository;

        public NotificationServiceImp(IDynamicRepository repository)
        {
            this.repository = repository;
        }

        public async Task Notify()
        {
            var sessionIds = await repository.FetchAsync<string>(new GetNotificationQueueCriteria());
            foreach (var sessionId in sessionIds)
            {
                await NotifyInternal(sessionId);
            }
        }

        public async Task Notify(string sessionId)
        {
            await NotifyInternal(sessionId);
        }

        public async Task Notify(string sessionId, int preMatchIssueId)
        {
            await NotifyInternal(sessionId, preMatchIssueId);
        }

        private async Task NotifyInternal(string sessionId, int preMatchIssueId = 0)
        {
            var tuple = await QueryBySession(sessionId);
            tuple.Item1.Select(user =>
            {
                var messages = tuple.Item2.Where(x => x.UserId == user.UserId).Select(x => new PreMatchIssueNotificationMessage
                {
                    UserId = x.UserId,
                    IsImportant = x.IsImportant,
                    MessageType = x.MessageType,
                    MessageContent = x.MessageContent,
                    PreMatchIssueId = preMatchIssueId
                }).ToList();

                return new NotificationEventArgs
                {
                    UserId = user.UserName,
                    Messages = messages.ConvertAll(x => (Message)x)
                };
            }).ToList().ForEach(arg =>
            {
                //NotificationEngine.Singleton.Notify(arg);
            });
        }

        private async Task<Tuple<IList<User>, IList<Message>>> QueryBySession(string sessionId)
        {
            var criteria = new GetMessageBySessionCriteria
            {
                SessionId = sessionId
            };
            criteria.ThrowExceptionIfInvalid();
            var result = await repository.FetchMultipleAsync<User, Message>(criteria);
            return new Tuple<IList<User>, IList<Message>>(result.Item1.ToList(), result.Item2.ToList());
        }
    }
}
