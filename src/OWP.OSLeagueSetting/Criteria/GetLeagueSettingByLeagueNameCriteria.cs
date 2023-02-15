namespace OWP.OSLeagueSetting.Criteria;

using Fanex.Data.Repository;

public class GetLeagueSettingByLeagueNameCriteria : CriteriaBase
{
    public string LeagueName { get; set; }

    public  int? LeagueSettingId { get; set; }

    public override string GetSettingKey() => "OWP_OSLeagueSetting_GetLeagueSettingByLeagueName";

    public override bool IsValid() => true;
}