using OWP.Models;

namespace OWP.TicketCenter.Notification.Models
{
    public class NotificationModel : MessageModel
    {
        public int TicketId { get; set; }

        public int TicketType { get; set; }

        public int EventType { get; set; }

        public int? TotalMessageUnread { get; set; }

        public bool IsNewMessage { get; set; }
    }
}