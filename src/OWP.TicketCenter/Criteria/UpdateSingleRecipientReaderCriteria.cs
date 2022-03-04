using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria
{
    public class UpdateSingleRecipientReaderCriteria : NonQueryCommand
    {
        public int TicketId { get; set; }

        public int RecipientId { get; set; }

        public bool IsRead { get; set; }

        public override string GetSettingKey() => "OWP_TicketCenter_UpdateSingleRecipientReader";

        public override bool IsValid() => TicketId > 0 && RecipientId > 0;
    }
}