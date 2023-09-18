using Fanex.Data.Repository;

namespace OWP.EventPicking.Criteria;

public class QueryLeagueNameByDateCriteria : CriteriaBase
{
    public DateTime Date { get; set; }

    public override string GetSettingKey() => "OWP_EventPicking_GetLeagueNameByDate";

    public override bool IsValid() => true;
}
