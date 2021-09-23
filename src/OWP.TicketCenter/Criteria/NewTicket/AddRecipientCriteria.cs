using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria.NewTicket
{
    public class AddRecipientCriteria : NonQueryCommand
    {
        public int TicketId { get; set; }

        public int RecipientType { get; set; }

        public string RecipientIds { get; set; }

        public override string GetSettingKey()
            => "OWP_TicketIssue_AddRecipient";

        public override bool IsValid()
            => TicketId > 0 && RecipientIds.Length > 0;
    }
}
