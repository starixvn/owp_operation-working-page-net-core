namespace OWP.SimpleDataApi.Criteria.PreMatchAssignment;

public class UpdateIssueIsReadCriteria : NonQueryCommand
{
    public int PreMatchIssueId { get; set; }

    public int ActorId { get; set; }

    public bool IsRead { get; set; }

    public override string GetSettingKey() => "OWP_PreMatchAssignment_UpdateIssueIsRead";

    public override bool IsValid() => PreMatchIssueId > 0 && ActorId > 0;
}