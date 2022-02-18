namespace OWP.TicketTypeRecipient.Models
{
    public class TicketTypeRecipient
    {
        public TicketType TicketType { get; set; }

        public List<Recipient> Recipients { get; set; }
    }
}