namespace OWP.SimpleDataApi.Criteria.OsLeagueSetting;

internal class QueryLeagueSettingDetailCriteria : CriteriaBase
{
    public int LeagueSettingId { get; set; }

    public override bool IsValid() => true;

    public override string GetSettingKey() => "OWP_OSLeagueSetting_GetLeagueSettingDetail";

}
