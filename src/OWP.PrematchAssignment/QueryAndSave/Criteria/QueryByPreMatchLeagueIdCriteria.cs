using Fanex.Data.Repository;

namespace OWP.PrematchAssignment.Criteria;

public class QueryByPreMatchLeagueIdCriteria : CriteriaBase
{
    public int PreMatchLeagueId { get; set; }

    public TimeSpan? StartTime { get; set; }

    public int ActorId { get; set; }

    public override string GetSettingKey() => "OWP_PreMatchAssignment_QueryByPreMatchLeagueId";

    public override bool IsValid() => PreMatchLeagueId > 0 && ActorId > 0;
}