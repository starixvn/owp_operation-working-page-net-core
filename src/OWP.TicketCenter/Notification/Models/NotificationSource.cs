using OWP.TicketCenter.Models;
using System.Collections.Generic;

namespace OWP.TicketCenter.Notification.Models
{
    public class NotificationSource
    {
        public string SessionId { get; set; }

        public int TicketId { get; set; }

        public int EventType { get; set; }
        
        public bool IsNewMessage { get; set; }
    }
}
