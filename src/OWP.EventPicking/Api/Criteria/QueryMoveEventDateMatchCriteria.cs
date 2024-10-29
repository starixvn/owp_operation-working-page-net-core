using Fanex.Data.Repository;

namespace OWP.EventPicking.Api.Criteria;

public class QueryMoveEventDateMatchCriteria : CriteriaBase
{
    public DateTime Date { get; set; }

    public override string GetSettingKey() => "OWP_EventPicking_GetMovedEventDateInfoSiteMatch";

    public override bool IsValid() => true;
}