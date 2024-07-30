using Fanex.Data.Repository;

namespace OWP.EventPicking.Api.Criteria;

public class GetLeagueSettingCriteria : CriteriaBase
{
    public override string GetSettingKey() => "OWP_EventPicking_GetLeagueSetting";

    public override bool IsValid() => true;
}