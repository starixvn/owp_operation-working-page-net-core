namespace OWP.PreMatchAssignment.Criteria
{
    using Fanex.Data.Repository;

    public class QueryIssueMessagesCriteria : CriteriaBase
    {
        public int ActorId { get; set; }

        public int PreMatchIssueId { get; set; }

        public override string GetSettingKey() => "OWP_PreMatchAssignment_QueryIssueMessages";

        public override bool IsValid() => PreMatchIssueId > 0;
    }
}