namespace OWP.Event.LiveGameAssignment.Download.Criteria.TaskListToday;

using Fanex.Data.Repository;

public class UpdateMatchNoteViewCriteria : CriteriaBase
{
    public int UserId { get; set; }

    public int MatchId { get; set; }

    public override string GetSettingKey() => "OWP_MatchNoteViewUpdate";

    public override bool IsValid() => UserId > 0 && MatchId > 0;
}