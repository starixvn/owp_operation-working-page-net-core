using System;

namespace OWP.TicketCenter.ViewModels
{
    public class MessageDetail
    {
        public int MessageId { get; set; }

        public int MessageContent { get; set; }

        public int UserId { get; set; }

        public int Username { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
