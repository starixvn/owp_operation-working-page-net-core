namespace OWP.Maxbet.Criteria;

using Fanex.Data.Repository;

public class GetMaxbetSettingCriteria : CriteriaBase
{
    public override string GetSettingKey() => "OWP_MaxbetSetting_Get";

    public override bool IsValid() => true;
}