
using Fanex.Data.Repository;

namespace OWP.EventPicking.Api.Criteria;
public class QueryEventPickingChildMatchCriteria : CriteriaBase
{
    public DateTime Date { get; set; }

    public override string GetSettingKey() => "OWP_EventPicking_GetEventPickingChildMatch";

    public override bool IsValid() => true;
}
