namespace OWP.TicketCenter.Criteria
{
    public class AddSingleRecipientCriteria: InsertOrDeleteSingleRecipientCriteria
    {
        public override string GetSettingKey()
            => "OWP_TicketIssue_AddSingleRecipient";
    }
}
