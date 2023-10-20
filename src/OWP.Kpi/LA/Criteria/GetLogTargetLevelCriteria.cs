using Fanex.Data.Repository;

namespace OWP.Kpi.LA.Criteria;

public class GetLogTargetLevelCriteria : CriteriaBase
{
    public int UserId { get; set; }

    public override string GetSettingKey() => "OWP_Kpi_GetLogTargetLevel";

    public override bool IsValid() => true;
}
