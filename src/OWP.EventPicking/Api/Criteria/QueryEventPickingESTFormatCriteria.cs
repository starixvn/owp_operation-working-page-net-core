using Fanex.Data.Repository;

namespace OWP.EventPicking.Api.Criteria;
public class QueryEventPickingESTFormatCriteria : CriteriaBase
{
    public DateTime Date { get; set; }

    public override string GetSettingKey() => "OWP_EventPicking_GetInfoSiteMatchForLST";

    public override bool IsValid() => true;
}
