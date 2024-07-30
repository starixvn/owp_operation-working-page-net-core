namespace OWP.WorkingShift.DailyWorkingShift.Models;

public class Assignment
{
    public int AssignmentId { get; set; }

    public int OWPMatchId { get; set; }

    public int Bettype { get; set; }

    public int UserId { get; set; }
}