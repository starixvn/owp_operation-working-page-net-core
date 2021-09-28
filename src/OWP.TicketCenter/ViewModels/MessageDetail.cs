using System;

namespace OWP.TicketCenter.ViewModels
{
    public class MessageDetail
    {
        public int MessageId { get; set; }

        public string Message { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
