namespace OWP.TicketCenter.Models
{
    using System.Collections.Generic;

    public abstract class EventTicketBase
    {
        public int TicketId { get; set; }

        public int TicketType { get; set; }

        public string TypeName { get; set; }

        public List<Comment> Comments { get; set; }
    }
}