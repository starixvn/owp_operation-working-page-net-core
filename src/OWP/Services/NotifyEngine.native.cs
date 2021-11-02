namespace OWP.Services
{
    using System.Linq;
    using Fanex.Data.Repository;
    using OWP.Models;
    using OWP.Utils;
    using Microsoft.AspNetCore.SignalR;
    using OWP.Criteria;
    using OWP.Hubs;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface NotifyEngine
    {
        Task Notify<T>(IList<T> messages, IList<string> receivedUserIds) where T : MessageModel;

        Task Notify<T>(string sessionId, Func<Tuple<IList<User>, IList<MessageModel>>, IList<T>> messageModelFunc) where T : MessageModel;
    }

    public class NotifyEngineImp : NotifyEngine
    {
        private readonly IHubContext<ChangedInfoNotifyHubImp, ChangedInfoNotifyHub> notifyChangedHub;

        private readonly IDynamicRepository repository;

        public NotifyEngineImp(
            IHubContext<ChangedInfoNotifyHubImp, ChangedInfoNotifyHub> notifyChangedHub,
            IDynamicRepository repository)
        {
            this.notifyChangedHub = notifyChangedHub;
            this.repository = repository;
        }

        public async Task Notify<T>(IList<T> messages, IList<string> receivedUserIds) where T : MessageModel
        {
            foreach (var message in messages)
            {
                var result = new
                {
                    notificationId = Guid.NewGuid(),
                    message
                };

                await notifyChangedHub.Clients
                    .Users(receivedUserIds)
                    .notifyChangedInformationData(result);
            }
        }

        public async Task Notify<T>(string sessionId, Func<Tuple<IList<User>, IList<MessageModel>>, IList<T>> messageModelFunc) where T : MessageModel
        {
            var userTuple = await QueryBySession(sessionId);

            var messageModel = messageModelFunc(userTuple);
            var receivedUserIds = userTuple
                .Item1
                .Select(u => u.UserId.ToString())
                .Distinct()
                .ToList();

            await Notify(messageModel, receivedUserIds);
        }

        private async Task<Tuple<IList<User>, IList<MessageModel>>> QueryBySession(string sessionId)
        {
            var criteria = new GetMessageBySessionCriteria
            {
                SessionId = sessionId
            };
            criteria.ThrowExceptionIfInvalid();
            var result = await repository.FetchMultipleAsync<User, MessageModel>(criteria);
            return new Tuple<IList<User>, IList<MessageModel>>(result.Item1.ToList(), result.Item2.ToList());
        }
    }
}