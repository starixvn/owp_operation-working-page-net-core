namespace OWP.AssignmentUpload.Submit.Models;

public class MatchKickoffDate
{
    public int MatchId { get; set; }

    public DateTime KickoffDate { get; set; }

    public int? MinorBettypeId { get; set; }
}