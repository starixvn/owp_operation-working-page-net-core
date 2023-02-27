using Fanex.Data.Repository;

namespace OWP.EventPicking.Criteria;

public class QueryMainMatchCriteria : CriteriaBase
{
    public DateTime Date { get; set; }

    public override string GetSettingKey() => "OWP_EventPicking_GetInfoSiteMatch";

    public override bool IsValid() => true;
}
