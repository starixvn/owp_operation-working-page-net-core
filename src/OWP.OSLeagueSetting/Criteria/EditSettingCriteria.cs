namespace OWP.OSLeagueSetting.Criteria;

public class EditSettingCriteria : AddOrEditSettingCommonCriteria
{
    public int LeagueSettingId { get; set; }

    public override string GetSettingKey() => "OWP_OSLeagueSetting_Edit";

    public override bool IsValid() => LeagueSettingId > 0;
}