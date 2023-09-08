using Fanex.Data.Repository;

namespace OWP.EventPicking.Criteria;

public class GetLogCriteria : CriteriaBase
{
    public int InfoSiteMatchId { get; set; }

    public override string GetSettingKey()
        => "OWP_EventPicking_GetLog";

    public override bool IsValid() => true;
}