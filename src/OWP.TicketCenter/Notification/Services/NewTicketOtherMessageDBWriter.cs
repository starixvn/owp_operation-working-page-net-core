using System.Threading.Tasks;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Notification.Const;
using OWP.TicketCenter.Notification.Models;

namespace OWP.TicketCenter.Notification.Services
{
    public interface NewTicketOtherMessageDBWriter
    {
        public Task<string> Execute(MessageTransform message, NotificationTemplateCode code);
    }

    public class NewTicketOtherMessageDBWriterImp : NewTicketOtherMessageDBWriter
    {
        private readonly Repository repository;

        public NewTicketOtherMessageDBWriterImp(Repository repository)
        {
            this.repository = repository;
        }

        public async Task<string> Execute(MessageTransform message, NotificationTemplateCode code)
        {
            var template = MessageTemplate.NotificationTemplateMap[(int)code];
            var criteria = new InsertNewTicketOtherNotificationCriteria
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

            await repository.InsertNewTicketOtherNotification(criteria);

            return criteria.SessionId;
        }
    }
}