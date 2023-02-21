using Fanex.Data.Repository;

namespace OWP.SimpleDataApi.Criteria.LeagueManagement;

public class LeagueManagementInsertLeagueSettingCriteria : NonQueryCommand
{
    public int ActorId { get; set; }

    public int OWPLeagueId { get; set; }

    public override string GetSettingKey() => "OWP_LeagueManagement_Insert";

    public override bool IsValid() => OWPLeagueId > 0 && ActorId > 0;
}