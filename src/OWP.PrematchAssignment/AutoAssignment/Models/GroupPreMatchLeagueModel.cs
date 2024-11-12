namespace OWP.PrematchAssignment.Models;

public class GroupPreMatchLeagueModel
{
    public IEnumerable<LeagueAssignment> MajorLeagues { get; set; } = new List<LeagueAssignment>();
    public IEnumerable<LeagueAssignment> GreenDotLeagues { get; set; } = new List<LeagueAssignment>();
    public IEnumerable<LeagueAssignment> NoGreenDotLeagues { get; set; } = new List<LeagueAssignment>();
    public IEnumerable<LeagueAssignment> FantasySabaLeagues { get; set; } = new List<LeagueAssignment>();
}