namespace OWP.SimpleDataApi.Criteria.LeagueManagement
{
    using Fanex.Data.Repository;

    public class LeagueManagementGetLeagueSettingByLeagueIdCriteria : CriteriaBase
    {
        public int OWPLeagueId { get; set; }

        public override string GetSettingKey() => "OWP_LeagueManagement_GetByLeagueId";

        public override bool IsValid() => OWPLeagueId > 0;
    }
}