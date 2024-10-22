namespace OWP.WorkingShift.SyncWorkingShift.Criteria;

public class GetEmployeeWorkingShiftHRPPCriteria : GetWorkingShiftCriteria
{
    public override string GetSettingKey() => "OWP_API_SyncWorkingShift_To_HRPP";

    public override bool IsValid() => true;
}