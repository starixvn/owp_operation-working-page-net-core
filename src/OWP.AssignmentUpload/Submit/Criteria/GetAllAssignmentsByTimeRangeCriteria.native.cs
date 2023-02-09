namespace OWP.AssignmentUpload.Submit.Criteria;

using Fanex.Data.Repository;

public class GetAllAssignmentsByTimeRangeCriteria : CriteriaBase
{
    public DateTime EventDate { get; set; }

    public override string GetSettingKey() => "OWP_AssignmentUpload_AllAssignmentsByTimeRange";

    public override bool IsValid() => true;
}