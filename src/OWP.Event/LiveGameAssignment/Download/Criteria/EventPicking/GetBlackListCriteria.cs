using Fanex.Data.Repository;

namespace OWP.Event.LiveGameAssignment.Download.Criteria.EventPicking;

public class GetBlackListCriteria : CriteriaBase
{
    public string LeagueNames { get; set; }

    public override string GetSettingKey() => "OWP_EventPicking_BlackListByLeagueNames";

    public override bool IsValid() => true;
}
