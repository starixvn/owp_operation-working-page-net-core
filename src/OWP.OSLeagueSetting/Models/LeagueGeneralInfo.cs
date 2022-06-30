namespace OWP.OSLeagueSetting.Models;

public class LeagueGeneralInfo
{
    public int LeagueId { get; set; }

    public string Category { get; set; }

    public string LeagueName { get; set; }

    public bool Active { get; set; }

    public string[] Logs { get; set; }
}