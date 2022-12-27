namespace OWP.EventPicking.Criteria;

using Fanex.Data.Repository;

public class GetLeagueSettingCriteria : CriteriaBase
{
    public override string GetSettingKey() => "OWP_EventPicking_GetLeagueSetting";

    public override bool IsValid() => true;
}