using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria
{
    public abstract class GetEventTicketBaseCriteria : CriteriaBase
    {
        public int ActorId { get; set; }

        public string Keyword { get; set; }

        public string TicketId { get; set; }

        public int TicketTypeId { get; set; } = 0;

        public override bool IsValid()
            => ActorId > 0;
    }
}
