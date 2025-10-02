using Fanex.Data.Repository;

namespace OWP.NewLeagueSetting.Criteria;
public class QueryLogCriteria : CriteriaBase
{
    public int SettingId { get; set; }

    public override string GetSettingKey()
        => "OWP_NewLeagueSetting_GetLog";

    public override bool IsValid() => SettingId > 0;

}