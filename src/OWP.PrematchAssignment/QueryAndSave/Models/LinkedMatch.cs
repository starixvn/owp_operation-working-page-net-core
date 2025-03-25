namespace OWP.PrematchAssignment.QueryAndSave.Models;

public class LinkedMatch
{
    public int InfositeMatchId { get; set; }

    public int OWPMatchId { get; set; }

    public string MatchStatus { get; set; } = string.Empty;
}