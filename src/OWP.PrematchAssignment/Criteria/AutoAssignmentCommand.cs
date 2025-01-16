using Fanex.Data.Repository;

namespace OWP.PrematchAssignment.Criteria;

public class AutoAssignmentCommand : NonQueryCommand
{
    public int ActorId { get; set; }
    public string Assignments { get; set; }

    public override string GetSettingKey() => "OWP_PreMatchAssignment_AutoAssign";

    public override bool IsValid() => ActorId > 0 && !string.IsNullOrWhiteSpace(Assignments);
}