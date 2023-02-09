namespace OWP.MaxbetSetting.Criteria;

using Fanex.Data.Repository;

public class QueryMaxbetSettingCriteria : CriteriaBase
{
    public override string GetSettingKey() => "OWP_MaxbetSetting_Get";

    public override bool IsValid() => true;
}
