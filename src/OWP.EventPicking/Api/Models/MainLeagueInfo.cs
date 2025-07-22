namespace OWP.EventPicking.Api.Models;

public class main_league_info
{
    public string TeamName { get; set; }
    public string MainLeagueName { get; set; }
}

public record MainLeagueInfo(string TeamName, string MainLeagueName);
