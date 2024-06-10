namespace OWP.SimpleDataApi.Criteria.EmployeeWorkingShift;



public class EmployeeWorkingShiftInsertNoteCriteria : CriteriaBase
{
    public int ActorId { get; set; }

    public int UserId { get; set; }

    public DateTime ShiftDate { get; set; }

    public string Note { get; set; }

    public override bool IsValid() => ActorId > 0 && UserId > 0 && Note != null;

    public override string GetSettingKey() => "OWP_EmployeeWorkingShift_InsertNote";
}