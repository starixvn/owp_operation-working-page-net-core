namespace OWP.TicketCenter.Criteria;

using Fanex.Data.Repository;

public abstract class RecipientCriteriaBase : NonQueryCommand
{
    public int TicketId { get; set; }

    public int RecipientType { get; set; }

    public string RecipientIds { get; set; }
}