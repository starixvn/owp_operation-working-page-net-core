namespace OWP.WorkingShift.Common.Criteria;

using Fanex.Data.Repository;
using System;

public class UpdateWorkingShiftCodeCriteria : NonQueryCommand
{
    public int ActorId { get; set; }

    public int EmployeeId { get; set; }

    public int ShiftId { get; set; }

    public DateTime ShiftDate { get; set; }

    public int? RefShiftId { get; set; }

    public bool IsOvertime { get; set; }

    public override string GetSettingKey() => "OWP_EmployeeWorkingShift_UpdateShiftCode";

    public override bool IsValid() => ActorId > 0 && EmployeeId > 0 && ShiftId > 0 && ShiftDate != DateTimeOffset.MinValue;
}