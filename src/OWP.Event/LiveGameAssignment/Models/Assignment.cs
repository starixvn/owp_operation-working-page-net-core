namespace OWP.Event.LiveGameAssignment.Models;

public class Assignment
{
    public int AssignmentId { get; set; }

    public int OWPMatchId { get; set; }

    public int? ParentMatchId { get; set; }

    public int Bettype { get; set; }

    public int? MinorBettypeId { get; set; }

    public int AssignmentType { get; set; }

    public int UserId { get; set; }

    public string Username { get; set; }

    public string AssignmentNote { get; set; }
}