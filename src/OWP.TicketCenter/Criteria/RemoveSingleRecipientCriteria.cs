namespace OWP.TicketCenter.Criteria
{
    public class RemoveSingleRecipientCriteria : InsertOrDeleteSingleRecipientCriteria
    {
        public override string GetSettingKey() => "OWP_TicketIssue_RemoveSingleRecipient";
    }
}