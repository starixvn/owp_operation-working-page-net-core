namespace OWP.Event.LiveGameAssignment.Download.Models;

public class MatchTrader
{
    public int OWPMatchId { get; set; }

    public int Bettype { get; set; }

    public int? MinorBettypeId { get; set; }

    public string Username { get; set; }

    public short AssignmentType { get; set; }

    public string AssignmentNote { get; set; }

    public int ParentMatchId { get; set; }
}