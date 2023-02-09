namespace OWP.SimpleDataApi.Criteria.LeagueManagement;

using Fanex.Data.Repository;

public class LeagueManagementDeleteLeagueSettingCriteria : NonQueryCommand
{
    public int ActorId { get; set; }

    public int LeagueSettingId { get; set; }

    public override string GetSettingKey() => "OWP_LeagueManagement_Delete";

    public override bool IsValid() => ActorId > 0 && LeagueSettingId > 0;
}