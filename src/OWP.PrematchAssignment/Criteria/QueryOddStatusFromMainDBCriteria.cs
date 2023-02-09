namespace OWP.PreMatchAssignment.Criteria;

using Fanex.Data.Repository;

public class QueryOddStatusFromMainDBCriteria : CriteriaBase
{
    public string AdminSiteMatchIds { get; set; }

    public override string GetSettingKey() => "OWP_PrematchAssignment_GetOddStatusFromMainDB";

    public override bool IsValid() => true;
}