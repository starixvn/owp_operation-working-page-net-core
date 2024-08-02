using Fanex.Data.Repository;

namespace OWP.NewLeagueSetting.Criteria;
public class RemoveSettingCriteria: NonQueryCommand
{
    public int LeagueSettingId { get; set; }
    public int ActorId { get; set; }

    public override string GetSettingKey()
        => "OWP_NewLeagueSetting_RemoveLeagueSetting";

    public override bool IsValid() => LeagueSettingId > 0 && ActorId>0;
    
}
