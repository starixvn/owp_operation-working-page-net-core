using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria.NewTicket
{
    public class AddNewTicketIssueEventCriteria : NonQueryCommand
    {
        // System auto set, FE do not care
        public int TicketId { get; set; }

        public int OwpLeagueId { get; set; }

        public int OwpHomeId { get; set; }

        public int OwpAwayId { get; set; }

        public override string GetSettingKey()
            => "OWP_TicketIssueEvent_Create";

        public override bool IsValid()
            => TicketId > 0 &&
               OwpLeagueId > 0 &&
               OwpHomeId > 0 &&
               OwpAwayId > 0;

    }
}
