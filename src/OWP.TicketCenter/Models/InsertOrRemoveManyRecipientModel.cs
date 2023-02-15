namespace OWP.TicketCenter.Models;

public class InsertOrRemoveManyRecipientModel
{
    public IList<int> RecipientIds { get; set; } = new List<int>();

    public int RecipientType { get; set; }
}