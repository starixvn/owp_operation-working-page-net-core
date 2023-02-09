using Fanex.Data.Repository;

namespace OWP.ApiSyncWorkingShift.Criteria;

public class GetEmployeeWorkingShiftHRPPCriteria : CriteriaBase
{
    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public override string GetSettingKey() => "OWP_API_SyncWorkingShift_To_HRPP";

    public override bool IsValid() => true;
}