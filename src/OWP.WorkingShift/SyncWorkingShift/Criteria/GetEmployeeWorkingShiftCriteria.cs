namespace OWP.WorkingShift.SyncWorkingShift.Criteria;

public class GetEmployeeWorkingShiftCriteria : CriteriaBase
{
    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public override string GetSettingKey() => "OWP_API_SyncEmployeeWorkingShift_GetWorkingShift";

    public override bool IsValid() => true;
}