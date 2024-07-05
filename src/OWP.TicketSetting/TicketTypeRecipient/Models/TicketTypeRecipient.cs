namespace OWP.TicketSetting.TicketTypeRecipient.Models;

public class TicketTypeRecipient
{
    public TicketType TicketType { get; set; }

    public List<Recipient> Recipients { get; set; }

    public List<Subtype> Subtypes { get; set; }
}