namespace OWP.PreMatchAssignment.Criteria;

using Fanex.Data.Repository;

public class QueryPreMatchIssueInfoCriteria : CriteriaBase
{
    public int PreMatchIssueId { get; set; }

    public override string GetSettingKey() => "OWP_PreMatchAssignment_QueryIssueInfo";

    public override bool IsValid() => PreMatchIssueId > 0;
}