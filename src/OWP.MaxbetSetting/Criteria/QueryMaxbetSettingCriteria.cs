using Fanex.Data.Repository;

namespace OWP.MaxbetSetting.Criteria;

public class QueryMaxbetSettingCriteria : CriteriaBase
{
    public override string GetSettingKey() => "OWP_MaxbetSetting_Get";

    public override bool IsValid() => true;
}
