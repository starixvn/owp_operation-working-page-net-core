
using Fanex.Data.Repository;

namespace OWP.EventPicking.Api.Criteria;

public class QueryChildMatchESTFormatCriteria:CriteriaBase
{
    public DateTime Date { get; set; }

    public override string GetSettingKey() => "OWP_EventPicking_GetEventPickingChildMatchForLST";

    public override bool IsValid() => true;
}
