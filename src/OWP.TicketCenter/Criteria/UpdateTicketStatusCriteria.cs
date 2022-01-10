using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria
{
    public class UpdateTicketStatusCriteria : NonQueryCommand
    {
        public int TicketId { get; set; }

        public string TicketStatus { get; set; }

        public int ModifiedBy { get; set; }

        public override string GetSettingKey() => "OWP_TicketCenter_UpdateTicketStatus";

        public override bool IsValid() => TicketId > 0 && ModifiedBy > 0;
    }
}