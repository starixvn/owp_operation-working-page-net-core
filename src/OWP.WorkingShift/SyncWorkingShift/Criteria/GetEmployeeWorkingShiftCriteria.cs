namespace OWP.WorkingShift.SyncWorkingShift.Criteria;

public class GetEmployeeWorkingShiftCriteria : GetWorkingShiftCriteria
{
    public override string GetSettingKey() => "OWP_API_SyncEmployeeWorkingShift_GetWorkingShift";

    public override bool IsValid() => true;
}