namespace OWP.SimpleDataApi.Criteria.LeagueManagement;

public class LeagueManagementUpdateLeagueGroupCriteria : NonQueryCommand
{
    public int ActorId { get; set; }

    public int OWPLeagueId { get; set; }

    public int? LeagueGroupId { get; set; }

    public override string GetSettingKey() => "OWP_OWPLeague_UpdateGroup";

    public override bool IsValid() => ActorId > 0 && OWPLeagueId > 0;
}