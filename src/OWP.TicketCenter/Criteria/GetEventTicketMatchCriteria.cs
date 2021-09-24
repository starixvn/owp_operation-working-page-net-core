namespace OWP.TicketCenter.Criteria
{
    using Fanex.Data.Repository;

    public class GetEventTicketMatchCriteria : NonQueryCommand
    {
        public int ActorId { get; set; }

        public string MatchKeyword { get; set; }

        public string LeagueKeyword { get; set; }

        public override string GetSettingKey()
            => "OWP_TicketCenterEvent_Get";

        public override bool IsValid()
            => true;
    }
}