namespace OWP.AssignmentUpload.Submit.Criteria;

using Fanex.Data.Repository;

public class GetAllMatchesSettingsByTimeRangeCriteria : CriteriaBase
{
    public DateTime EventDate { get; set; }

    public override string GetSettingKey() => "OWP_AssignmentUpload_AllMatchSettingsByTimeRange";

    public override bool IsValid() => true;
}