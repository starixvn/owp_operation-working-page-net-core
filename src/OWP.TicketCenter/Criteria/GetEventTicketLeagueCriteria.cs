namespace OWP.TicketCenter.Criteria
{
    using Fanex.Data.Repository;

    public class GetEventTicketLeagueCriteria : NonQueryCommand
    {
        public int ActorId { get; set; }

        public string LeagueKeyword { get; set; }

        public override string GetSettingKey()
            => "OWP_TicketCenterLeague_Get";

        public override bool IsValid()
            => true;
    }
}