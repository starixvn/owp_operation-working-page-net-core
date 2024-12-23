namespace OWP.WorkingShift.SyncWorkingShift.Criteria;

public class GetEmployeeWorkingShiftHRMCriteria : GetWorkingShiftCriteria
{
    public string LocationCode { get; set; }

    public override string GetSettingKey() => "OWP_API_SyncEmployeeWorkingShift";

    public override bool IsValid() => true;
}