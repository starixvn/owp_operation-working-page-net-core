namespace OWP.Event.LiveGameAssignment.Models;

public class ValidateToAddEventModel
{
    public int? ExistedMatchId { get; set; }

    public string MatchSettingBettypeIds { get; set; }

    public int? MinorBettypeId { get; set; }

    public int? ParentMatchId { get; set; }
}