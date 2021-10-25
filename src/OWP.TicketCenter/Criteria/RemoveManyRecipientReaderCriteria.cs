using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria
{
    public class RemoveManyRecipientReaderCriteria : NonQueryCommand
    {
        public int TicketId { get; set; }

        public string RecipientIds { get; set; }

        public override string GetSettingKey()
            => "OWP_TicketCenter_RemoveManyRecipientReader";

        public override bool IsValid()
            => TicketId > 0 && RecipientIds.Length > 0;
    }
}
