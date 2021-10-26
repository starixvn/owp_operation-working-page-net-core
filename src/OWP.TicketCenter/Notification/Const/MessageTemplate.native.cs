using OWP.TicketCenter.Notification.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace OWP.TicketCenter.Notification.Const
{
    public static class MessageTemplate
    {
        #region MessageTemplate
        private static readonly ImmutableList<NotificationTemplate> notificationTemplates = ImmutableList<NotificationTemplate>.Empty.AddRange(new[]
        {
            new NotificationTemplate
            {
                NotificationType = (int)NotificationTemplateCode.NewTicket,
                NotificationContent = "New ticket <p>#<ticketId></p> <Event>: <comment> by <username>",
                IsImportant = true
            },

            new NotificationTemplate
            {
                NotificationType = (int)NotificationTemplateCode.AddRecipient,
                NotificationContent = "You have been added to ticket <p>#<ticketId></p> <Event> by <username>",
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

        public static readonly Dictionary<int, NotificationTemplate> NotificationTemplateMap = notificationTemplates.ToDictionary(
            m => m.NotificationType,
            m => m);
    }
}
