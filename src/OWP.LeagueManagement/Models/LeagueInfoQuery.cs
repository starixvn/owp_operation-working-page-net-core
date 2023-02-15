namespace OWP.LeagueManagement.Models;

public class LeagueInfoQuery
{
    public string League { get; set; }

    public int LeagueGroupId { get; set; }

    public string Category { get; set; }

    public bool IsActive { get; set; }
}