using Fanex.Data.Repository;

namespace OWP.MaxbetSetting.Criteria;

public class GetLogMaxBetSettingCriteria : CriteriaBase
{
    public int FT_HDP { get; set; }

    public override string GetSettingKey() => "OWP_MaxBetSetting_GetLogMaxBetSetting";

    public override bool IsValid() => true;
}
