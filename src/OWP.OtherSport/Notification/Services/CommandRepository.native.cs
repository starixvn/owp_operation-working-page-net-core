namespace OWP.OtherSport.Notification.Services
{
    using Fanex.Data.Repository;
    using Fanex.NAP.Client.Helpers.Web;
    using Models;
    using Olympus.Xml;
    using OWP.OtherSport.Notification.Criteria;

    public interface CommandRepository
    {
        Task InsertMessage(List<MessageModel> messages, string sessionId);
    }

    public class CommandRepositoryImp : CommandRepository
    {
        private readonly IDynamicRepository repository;
        private readonly INapSession currentSession;

        public CommandRepositoryImp(
            INapSession currentSession,
            IDynamicRepository repository)
        {
            this.currentSession = currentSession;
            this.repository = repository;
        }

        public async Task InsertMessage(List<MessageModel> messages, string sessionId)
        {
            messages.ForEach(x => x.ReceivedUserIdList = string.Join(",", x.ReceivedUserIds));

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