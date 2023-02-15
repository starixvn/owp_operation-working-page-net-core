namespace OWP.OSLeagueSetting.Criteria;

using Fanex.Data.Repository;

public class GetLogSettingCriteria : CriteriaBase
{
    public int LeagueSettingId { get; set; }

    public override bool IsValid() => true;

    public override string GetSettingKey() => "OWP_OSLeagueSetting_GetLogSetting";
}
