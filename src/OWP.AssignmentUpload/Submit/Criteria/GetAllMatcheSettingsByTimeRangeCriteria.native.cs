using Fanex.Data.Repository;

namespace OWP.AssignmentUpload.Submit.Criteria;

public class GetAllMatchesSettingsByTimeRangeCriteria : CriteriaBase
{
    public DateTime EventDate { get; set; }

    public override string GetSettingKey() => "OWP_AssignmentUpload_AllMatchSettingsByTimeRange";

    public override bool IsValid() => true;
}