using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria
{
    public abstract class GetEventTicketCriteriaBase : CriteriaBase
    {
        public int ActorId { get; set; }

        public string Keyword { get; set; }

        public string TicketId { get; set; }

        public string TicketStatuses { get; set; }

        public string TicketTypeId { get; set; }

        public int MarketKey { get; set; } = 1;

        public override bool IsValid() => ActorId > 0;
    }
}