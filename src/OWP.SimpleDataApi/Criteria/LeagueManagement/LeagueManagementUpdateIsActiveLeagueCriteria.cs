namespace OWP.SimpleDataApi.Criteria.LeagueManagement
{
    using Fanex.Data.Repository;

    public class LeagueManagementUpdateIsActiveLeagueCriteria : NonQueryCommand
    {
        public int ActorId { get; set; }

        public int OWPLeagueId { get; set; }

        public bool IsActive { get; set; }

        public override string GetSettingKey() => "OWP_OWPLeague_UpdateIsActive";

        public override bool IsValid() => ActorId > 0 && OWPLeagueId > 0;
    }
}