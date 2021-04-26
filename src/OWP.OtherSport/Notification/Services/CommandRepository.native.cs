namespace OWP.OtherSport.Notification.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Data.Repository;
    using Fanex.NAP.Core.Web.Session;
    using Models;
    using Olympus.Xml;

    public interface CommandRepository
    {
        Task InsertMessage(List<MessageModel> messages, string sessionId);
    }

    public class CommandRepositoryImp : CommandRepository
    {
        private readonly IDynamicRepository repository;
        private readonly ICurrentSession currentSession;

        public CommandRepositoryImp(
            ICurrentSession currentSession,
            IDynamicRepository repository)
        {
            this.currentSession = currentSession;
            this.repository = repository;
        }

        public async Task InsertMessage(List<MessageModel> messages, string sessionId)
        {
            var criteria = new InsertMessageCommand
            {
                Messages = XmlConverter.Serialize(messages),
                CreatedByUserId = currentSession.UserId,
                SessionId = sessionId
            };

            await repository.ExecuteAsync(criteria);
        }
    }
}