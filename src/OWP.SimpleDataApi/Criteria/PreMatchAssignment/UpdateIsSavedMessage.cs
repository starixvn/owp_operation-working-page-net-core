namespace OWP.SimpleDataApi.Criteria.PreMatchAssignment;

public class UpdateIsSavedMessage : NonQueryCommand
{
    public int PreMatchIssueMessageId { get; set; }

    public bool IsSaved { get; set; }

    public override string GetSettingKey() => "OWP_PreMatchAssignment_UpdateIsSavedMessage";

    public override bool IsValid() => PreMatchIssueMessageId > 0;
}