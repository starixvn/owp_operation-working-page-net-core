using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria
{
    public abstract class InsertOrDeleteSingleRecipientCriteria: CriteriaBase
    {
        public int TicketId { get; set; }

        public int RecipientType { get; set; }

        public int RecipientId { get; set; }

        public override bool IsValid()
            => TicketId > 0 && RecipientId > 0;
    }
}
