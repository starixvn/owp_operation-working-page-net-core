namespace OWP.SimpleDataApi.Criteria.PreMatchAssignment;

public class ValidateAssignmentCriteria : CriteriaBase
{
    public string Assignee { get; set; }

    public override string GetSettingKey() => "OWP_PreMatchAssignment_ValidateAssignment";

    public override bool IsValid() => !string.IsNullOrEmpty(Assignee);
}