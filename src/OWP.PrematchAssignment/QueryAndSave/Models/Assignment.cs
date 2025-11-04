namespace OWP.PrematchAssignment.QueryAndSave.Models;

public class Assignment
{
    public int AssignmentId { get; set; }

    public int OWPMatchId { get; set; }

    public int Bettype { get; set; }

    public int UserId { get; set; }

    public string Username { get; set; }

    public string AssignmentNote { get; set; }
}