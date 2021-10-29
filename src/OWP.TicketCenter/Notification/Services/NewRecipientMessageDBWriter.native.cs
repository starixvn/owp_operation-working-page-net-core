using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Handlers;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Notification.Const;

namespace OWP.TicketCenter.Notification.Services
{
    public interface NewRecipientMessageDBWriter
    {
        Task<string> Execute(InsertSingleRecipientRequest recipientRequest);
    }

    public class NewRecipientMessageDBWriterImp : NewRecipientMessageDBWriter
    {
        private readonly Repository repository;

        public NewRecipientMessageDBWriterImp(Repository repository)
        {
            this.repository = repository;
        }

        public async Task<string> Execute(InsertSingleRecipientRequest recipientRequest)
        {
            var template = MessageTemplate.NotificationTemplateMap[(int)NotificationTemplateCode.AddRecipient];
            var criteria = new InsertNewRecipientNotificationCriteria()
            {
                ActorId = recipientRequest.ActorId,
                IsImportant = template.IsImportant,
                IsWatcher = template.IsWatcher,
                ReceivedUserId = recipientRequest.RecipientId,
                NotificationContent = template.NotificationContent
                    .Replace("<ticketId>", $"{recipientRequest.TicketId}")
                    .Replace("<Event>", recipientRequest.TicketTitle)
                    .Replace("<username>", recipientRequest.ActorUsername),
                NotificationType = template.NotificationType,
            };

            await repository.InsertNewRecipientNotification(criteria);

            return criteria.SessionId;
        }
    }
}