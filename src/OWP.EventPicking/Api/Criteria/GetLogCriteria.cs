using Fanex.Data.Repository;

namespace OWP.EventPicking.Api.Criteria;

public class GetLogCriteria : CriteriaBase
{
    public int InfoSiteMatchId { get; set; }

    public override string GetSettingKey()
        => "OWP_EventPicking_GetLogHistory";

    public override bool IsValid() => true;
}