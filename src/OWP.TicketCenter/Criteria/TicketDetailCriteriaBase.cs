using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria
{
    public abstract class TicketDetailCriteriaBase : CriteriaBase
    {
        public int TicketId { get; set; }

        public override bool IsValid() => TicketId > 0;
    }
}