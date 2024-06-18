namespace OWP.WorkingShift.Common.Criteria;

using System;
using Fanex.Data.Repository;

public class RemoveSingleShiftCriteria : NonQueryCommand
{
    public int ActorId { get; set; }

    public int EmployeeId { get; set; }

    public DateTime ShiftDate { get; set; }

    public override string GetSettingKey() => "OWP_EmployeeWorkingShift_RemoveShiftCode";

    public override bool IsValid() => ActorId > 0 && EmployeeId > 0;
}