using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Notification.Const;
using OWP.TicketCenter.Notification.Models;

namespace OWP.TicketCenter.Notification.Services
{
    public interface MessageToDbWriterServices
    {
        public Task<string> Execute(int actorId, Message message, NotificationTemplateCode code);
    }

    public class MessageToDbWriterServicesImp : MessageToDbWriterServices
    {
        #region MessageTemplate
        private static readonly ImmutableList<NotificationTemplate> notificationTemplates = ImmutableList<NotificationTemplate>.Empty.AddRange(new[]
        {
            new NotificationTemplate
            {
                NotificationType = (int)NotificationTemplateCode.NewTicket,
                NotificationContent = "New ticket <p>#<ticketId></p> <ticketType>: <comment> by <username>",
                IsImportant = true
            },

            new NotificationTemplate
            {
                NotificationType = (int)NotificationTemplateCode.AddRecipient,
                NotificationContent = "You have been added to ticket <p>#<ticketId></p> <ticketType> by <username>",
                IsImportant = true
            },
            new NotificationTemplate
            {
                NotificationType = (int)NotificationTemplateCode.NewMessage,
                NotificationContent = "New message from ticket <p>#<ticketId></p>: \"\"<comment>\"\" by <username>",
                IsImportant = true
            },
            new NotificationTemplate
            {
                NotificationType = (int)NotificationTemplateCode.NewAttachment,
                NotificationContent = "New attachment from ticket <p>#<ticketId></p> by <username>",
                IsImportant = true
            }
        });

        #endregion MessageTemplate

        private static readonly Dictionary<int, NotificationTemplate> notificationTemplateMap = notificationTemplates.ToDictionary(
            m => m.NotificationType,
            m => m);

        private readonly Repository repository;

        public MessageToDbWriterServicesImp(Repository repository)
        {
            this.repository = repository;
        }

        public async Task<string> Execute(int actorId, Message message, NotificationTemplateCode code)
        {
            var template = notificationTemplateMap[(int)code];
            var criteria = new InsertNotificationMessageCriteria
            {
                ActorId = actorId,
                IsImportant = template.IsImportant,
                IsWatcher = template.IsWatcher,
                NotificationContent = UpdateMessageContent(template.NotificationContent,message),
                NotificationType = template.NotificationType,
                PreMatchIssueId = message.PreMatchIssueId
            };

            await repository.InsertNewMessage(criteria);

            return criteria.SessionId;
        }

        private static string UpdateMessageContent(string messageContent, Message message)
            => messageContent
                .Replace("<ticketId>", message.TicketId.ToString())
                .Replace("ticketType", message.TicketType)
                .Replace("<comment>", message.Comment)
                .Replace("<username>", message.Username);
    }
}
