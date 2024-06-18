namespace OWP.WorkingShift.WeeklyWorkingShift.Criteria;

using Fanex.Data.Repository;

public class EmployeeWorkingShiftUploadCriteria : NonQueryCommand
{
    public int ActorId { get; set; }

    public string Data { get; set; }

    public override string GetSettingKey() => "OWP_EmployeeWorkingShift_Upload";

    public override bool IsValid() => ActorId > 0;
}
