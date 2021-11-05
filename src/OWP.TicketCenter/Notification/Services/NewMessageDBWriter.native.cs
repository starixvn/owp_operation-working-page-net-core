using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Handlers;
using OWP.TicketCenter.Notification.Const;

namespace OWP.TicketCenter.Notification.Services
{
    public interface NewMessageDBWriter
    {
        Task<string> Execute(NewMessageRequest newMessageRequest);
    }

    public class NewMessageDBWriterImp : NewMessageDBWriter
    {
        private readonly Repository repository;

        public NewMessageDBWriterImp(Repository repository)
        {
            this.repository = repository;
        }

        public async Task<string> Execute(NewMessageRequest newMessageRequest)
        {
            var templateCode = newMessageRequest.FileId.HasValue
                ? NotificationTemplateCode.NewAttachment
                : NotificationTemplateCode.NewMessage;
            var template = MessageTemplate.NotificationTemplateMap[(int)templateCode];

            var criteria = new InsertNewMessageNotificationCriteria
            {
                NotificationContent = template.NotificationContent
                    .Replace("<ticketId>", $"{newMessageRequest.TicketId}")
                    .Replace("<Event>", newMessageRequest.TicketTitle)
                    .Replace("<comment>", newMessageRequest.Message),
                NotificationType = template.NotificationType,
                IsImportant = template.IsImportant,
                IsWatcher = template.IsWatcher,
                TicketId = newMessageRequest.TicketId,
                ActorId = newMessageRequest.ActorId,
            };

            await repository.InsertNewMessageNotification(criteria);

            return criteria.SessionId;
        }
    }
}