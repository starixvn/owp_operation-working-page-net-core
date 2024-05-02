namespace OWP.NewLeagueSetting.Criteria;
public class QueryLeagueSettingDetailCriteria : CriteriaBase
{
    public int LeagueSettingId { get; set; }

    public override string GetSettingKey() => "OWP_NewLeagueSetting_QueryLeagueSettingDetail";

    public override bool IsValid() => true;
}


