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
                NotificationContent = "New ticket <b>#<ticketId></b> <Event>: <comment> by <username>",
                IsImportant = true
            },

            new NotificationTemplate
            {
                NotificationType = (int)NotificationTemplateCode.AddRecipient,
                NotificationContent = "You have been added to ticket <b>#<ticketId></b> <Event> by <username>",
                IsImportant = true
            },
            new NotificationTemplate
            {
                NotificationType = (int)NotificationTemplateCode.NewMessage,
                NotificationContent = "New message from ticket <b>#<ticketId></b>: \"\"<comment>\"\" by <username>",
                IsImportant = true
            },
            new NotificationTemplate
            {
                NotificationType = (int)NotificationTemplateCode.NewAttachment,
                NotificationContent = "New attachment from ticket <b>#<ticketId></b> by <username>",
                IsImportant = true
            }
        });

        #endregion MessageTemplate

        public static readonly Dictionary<int, NotificationTemplate> NotificationTemplateMap =
            notificationTemplates.ToDictionary(
            m => m.NotificationType,
            m => m);
    }
}