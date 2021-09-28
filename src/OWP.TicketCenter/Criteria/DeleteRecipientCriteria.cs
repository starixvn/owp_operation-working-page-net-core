using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria
{
    public class DeleteRecipientCriteria : RecipientCriteriaBase
    {
        public override string GetSettingKey()
            => "OWP_TicketIssue_DeleteRecipient";

        public override bool IsValid()
            => TicketId > 0 && RecipientIds.Length > 0;
    }
}
