namespace OWP.NewLeagueSetting.Criteria;

public class QueryLeagueSettingByLeagueIdCriteria : CriteriaBase
{
    public int OwpLeagueId { get; set; }

    public override string GetSettingKey() => "OWP_NewLeagueSetting_QueryLeagueSettingByLeagueId";

    public override bool IsValid() => true;
}