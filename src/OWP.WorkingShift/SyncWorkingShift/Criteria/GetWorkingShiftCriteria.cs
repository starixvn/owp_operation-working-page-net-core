namespace OWP.WorkingShift.SyncWorkingShift.Criteria;
public  abstract class GetWorkingShiftCriteria : CriteriaBase
{
    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public string AllUserJson { get; set; }
}
