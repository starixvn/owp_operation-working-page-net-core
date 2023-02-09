namespace OWP.LiveGameList.Models;

public class MatchNote
{
    public int OWPMatchId { get; set; }

    public string Username { get; set; }

    public string Note { get; set; }

    public bool IsViewed { get; set; }

    public System.DateTime LastUpdated { get; set; }
}