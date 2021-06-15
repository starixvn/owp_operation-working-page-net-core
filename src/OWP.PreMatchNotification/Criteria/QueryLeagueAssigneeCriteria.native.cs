namespace OWP.PreMatchNotification.Criteria
{
    using Fanex.Data.Repository;

    public class QueryLeagueAssigneeCriteria : CriteriaBase
    {
        public int PreMatchLeagueId { get; set; }

        public override string GetSettingKey() => "OWP_PreMatchNotification_QueryLeagueAssignee";

        public override bool IsValid() => true;
    }
}