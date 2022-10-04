using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria
{
    public class AddNewTicketIssueLgaCriteria : NonQueryCommand
    {
        public int TicketId { get; set; }

        public int OwpMatchId { get; set; }

        public override string GetSettingKey() => "OWP_TicketIssueLga_Create";

        public override bool IsValid()
            => TicketId > 0 && OwpMatchId > 0;
    }
}