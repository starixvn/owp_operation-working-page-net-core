using Fanex.Data.Repository;

namespace OWP.PrematchAssignment.Criteria;

public class GetMaxbetSpreadCriteria : CriteriaBase
{
    public string AdminSiteMatchId { get; set; }

    public override string GetSettingKey() => "OWP_PrematchAssignment_GetMaxbetSpread";

    public override bool IsValid() => true;
}