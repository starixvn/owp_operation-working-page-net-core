namespace OWP.Event.LiveGameAssignment.Models;

public class ValidateToEditEventModel
{
    public int OWPMatchId { get; set; }

    public int? MinorBettypeId { get; set; }

    public int? ParentMatchId { get; set; }
}