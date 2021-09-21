
namespace OWP.TicketTypeRecipient.Models
{
    using System.Collections.Generic;

    public class TicketTypeRecipient
    {
        public TicketType TicketType { get; set; }

        public List<Recipient> Recipients { get; set; }
    }
}