
namespace OWP.NewLeagueSetting.Criteria;
public class InsertLeagueSettingCriteria : InsertOrUpdateSettingCriteria
{
    public string League { get; set; }
    public string LogData { get; set; }

    public override string GetSettingKey() => "OWP_NewLeagueSetting_InsertLeagueSetting";

    public override bool IsValid() => League is not null;
}