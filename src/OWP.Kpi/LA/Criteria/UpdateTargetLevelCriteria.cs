using Fanex.Data.Repository;

namespace OWP.Kpi.LA.Criteria;

public class UpdateTargetLevelCriteria : CriteriaBase
{
    public string TargetJson { get; set; }

    public override string GetSettingKey() => "OWP_Kpi_UpdateTargetLevel";

    public override bool IsValid() => true;
}