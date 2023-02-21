using Fanex.Data.Repository;

namespace OWP.PrematchAssignment.Criteria;

public class QueryOddStatusFromMainDBCriteria : CriteriaBase
{
    public string AdminSiteMatchIds { get; set; }

    public override string GetSettingKey() => "OWP_PrematchAssignment_GetOddStatusFromMainDB";

    public override bool IsValid() => true;
}