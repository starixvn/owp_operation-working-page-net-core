using Fanex.Data.Repository;

namespace OWP.Event.LiveGameAssignment.Download.Criteria.EventPicking;

public class GetEventInfoCriteria : CriteriaBase
{
    public string InfoSiteIds { get; set; }

    public override string GetSettingKey() => "OWP_EventPicking_GetMatchInfoByInfositeIds";

    public override bool IsValid() => true;
}
