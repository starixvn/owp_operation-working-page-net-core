namespace OWP.PreMatchAssignment.Criteria
{
    using Fanex.Data.Repository;

    public class QueryIssueMessagesAfterInsertCriteria : CriteriaBase
    {
        public int? OWPLeagueId { get; set; }

        public int? InfoSiteMatchId { get; set; }

        public int ActorId { get; set; }

        public override string GetSettingKey() => "OWP_PreMatchAssignment_QueryIssueMessagesAfterInsert";

        public override bool IsValid() => OWPLeagueId > 0 || InfoSiteMatchId > 0;
    }
}