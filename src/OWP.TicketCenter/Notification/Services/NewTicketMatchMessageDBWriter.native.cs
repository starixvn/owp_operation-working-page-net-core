
using OWP.TicketCenter.Builder;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Notification.Const;
using OWP.TicketCenter.Notification.Models;

namespace OWP.TicketCenter.Notification.Services
{
    public interface NewTicketMatchMessageDBWriter
    {
        public Task<string> Execute(MessageTransform message, NotificationTemplateCode code);
    }

    public class NewTicketMatchMessageDBWriterImp : NewTicketMatchMessageDBWriter
    {
        private readonly Repository repository;

        public NewTicketMatchMessageDBWriterImp(Repository repository)
        {
            this.repository = repository;
        }

        public async Task<string> Execute(MessageTransform message, NotificationTemplateCode code)
        {
            var template = MessageTemplate.NotificationTemplateMap[(int)code];
            var criteria = new InsertNewTicketMatchNotificationCriteria()
            {
                ActorId = message.ActorId,
                IsImportant = template.IsImportant,
                IsWatcher = template.IsWatcher,
                NotificationContent = template.NotificationContent
                    .Replace("<ticketId>", message.TicketId.ToString())
                    .Replace("<comment>", message.Comment),
                NotificationType = template.NotificationType,
                TicketId = message.TicketId
            };

            await repository.InsertNewTicketMatchNotification(criteria);

            return criteria.SessionId;
        }
    }
}