using System.Collections.Generic;

namespace OWP.TicketCenter.Models
{
    public class NewTicketModelBase
    {
        // FE do not care this property
        public int TicketId { get; set; }

        public string Message { get; set; }

        public int UserId { get; set; }

        public IList<Recipient> Recipients { get; set; }
    }
}
