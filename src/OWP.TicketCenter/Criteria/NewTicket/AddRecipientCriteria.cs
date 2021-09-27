using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria.NewTicket
{
    public class AddRecipientCriteria : RecipientBaseCriteria
    {
        public override string GetSettingKey()
            => "OWP_TicketIssue_AddRecipient";

        public override bool IsValid()
            => TicketId > 0 && RecipientIds.Length > 0;
    }
}
