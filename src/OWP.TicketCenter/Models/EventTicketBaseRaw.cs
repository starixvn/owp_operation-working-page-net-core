using System;

namespace OWP.TicketCenter.Models
{
    public class EventTicketBaseRaw
    {
        public int TicketId { get; set; }

        public int TicketType { get; set; }

        public string TypeName { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Message { get; set; }
    }
}