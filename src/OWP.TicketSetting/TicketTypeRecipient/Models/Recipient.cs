namespace OWP.TicketSetting.TicketTypeRecipient.Models;

public class Recipient
{
    public int? TicketTypeRecipientId { get; set; }

    public int RecipientId { get; set; }

    public int RecipientType { get; set; }

    public string RecipientName { get; set; }

    public string SubtypeName { get; set; }
}