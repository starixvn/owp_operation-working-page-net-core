
namespace OWP.NewLeagueSetting.Criteria;
public class UpdateLeagueSettingCriteria : InsertOrUpdateSettingCriteria
{
   

    public int OWPLeagueId { get; set; }
    
    public int OWPLeagueId { get; set; }

    public override string GetSettingKey() => "OWP_NewLeagueSetting_UpdateLeagueSetting";

    public override bool IsValid() => LeagueSettingId>0;
}
