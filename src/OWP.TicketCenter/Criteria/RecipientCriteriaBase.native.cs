using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria;

public abstract class RecipientCriteriaBase : NonQueryCommand
{
    public int TicketId { get; set; }

    public int RecipientType { get; set; }

    public string RecipientIds { get; set; }
}