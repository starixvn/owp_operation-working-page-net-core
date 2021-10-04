using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria
{
    public class UpdateRecipientReaderCriteria : NonQueryCommand
    {
        public int TicketId { get; set; }

        public bool IsRead { get; set; }

        public override string GetSettingKey()
            => "OWP_TicketCenter_UpdateRecipientReader";


        public override bool IsValid()
            => TicketId > 0;
    }
}
