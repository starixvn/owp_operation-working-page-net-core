

namespace OWP.NewLeagueSetting.Criteria;
public class QueryLeagueSettingCriteria : CriteriaBase
{
    public string League { get; set; }

    public bool IsEqualLeague { get; set; }

    public override string GetSettingKey() => "OWP_NewLeagueSetting_QueryAllLeagueSetting";

    public override bool IsValid() => true;
}

