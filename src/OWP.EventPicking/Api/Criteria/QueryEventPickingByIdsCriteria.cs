namespace OWP.EventPicking.Api.Criteria;

public class QueryEventPickingByIdsCriteria : CriteriaBase
{
    public string InfoSiteIds { get; set; }

    public override string GetSettingKey() => "OWP_EventPicking_GetInfoSiteMatchByIds";

    public override bool IsValid() => true;

}