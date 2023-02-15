namespace OWP.TicketCenter.Criteria;

using Fanex.Data.Repository;

public abstract class TicketDetailCriteriaBase : CriteriaBase
{
    public int TicketId { get; set; }

    public override bool IsValid() => TicketId > 0;
}