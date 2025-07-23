namespace OWP.EventPicking.Api.Models;

public class main_league_info
{
    public string teamName { get; set; }
    public string mainLeague { get; set; }
}

public record MainLeagueInfo(string TeamName, string MainLeague);
