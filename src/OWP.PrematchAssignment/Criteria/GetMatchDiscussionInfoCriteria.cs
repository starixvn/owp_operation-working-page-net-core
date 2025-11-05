using Fanex.Data.Repository;

namespace OWP.PrematchAssignment.Criteria;

public class GetAssignmenstByMatchCriteria : CriteriaBase
{
    public int AdminSiteId { get; set; }

    public override string GetSettingKey() => "OWP_LiveGameAssignment_GetAssignments_ByMatch";

    public override bool IsValid() => AdminSiteId > 0;
}