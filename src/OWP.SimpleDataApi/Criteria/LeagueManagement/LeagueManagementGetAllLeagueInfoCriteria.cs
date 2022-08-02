namespace OWP.SimpleDataApi.Criteria.LeagueManagement
{
    using Fanex.Data.Repository;

    public class LeagueManagementGetAllLeagueInfoCriteria : CriteriaBase
    {
        public string League { get; set; }

        public int LeagueGroupId { get; set; }

        public string Category { get; set; }

        public bool IsActive { get; set; }

        public override string GetSettingKey() => "OWP_LeagueManagement_Get";

        public override bool IsValid() => true;
    }
}