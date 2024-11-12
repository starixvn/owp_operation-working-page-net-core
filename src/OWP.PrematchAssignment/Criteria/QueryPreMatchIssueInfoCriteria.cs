using Fanex.Data.Repository;

namespace OWP.PrematchAssignment.Criteria;

public class QueryPreMatchIssueInfoCriteria : CriteriaBase
{
    public int PreMatchIssueId { get; set; }

    public override string GetSettingKey() => "OWP_PreMatchAssignment_QueryIssueInfo";

    public override bool IsValid() => PreMatchIssueId > 0;
}