namespace OWP.TeamManagement.Model;
public class LeagueInfo
{
    public int LeagueId { get; set; }
    public string LeagueName { get; set; }
    public int LeagueGroupId { get; set; }
    public string LeagueGroupName { get; set; }
    public IEnumerable<TeamInfo> Teams { get; set; }
    public bool IsExpanded { get; set; }
}

