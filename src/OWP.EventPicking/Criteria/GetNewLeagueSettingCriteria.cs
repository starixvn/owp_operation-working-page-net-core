using Fanex.Data.Repository;

namespace OWP.EventPicking.Criteria;

public class GetNewLeagueSettingCriteria : CriteriaBase
{
    public string League { get; set; }

    public override string GetSettingKey() => "OWP_NewLeagueSetting_QueryAllLeagueSetting";

    public override bool IsValid() => true;
}