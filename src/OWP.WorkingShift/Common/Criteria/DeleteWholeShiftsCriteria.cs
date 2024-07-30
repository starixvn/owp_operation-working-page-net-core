namespace OWP.WorkingShift.Common.Criteria;

using System;
using Fanex.Data.Repository;

public class DeleteWholeShiftsCriteria : NonQueryCommand
{
    public int ActorId { get; set; }

    public int UserId { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public override bool IsValid() => ActorId > 0
        && UserId > 0
        && IsHasValidTimeRange();

    private bool IsHasValidTimeRange()
    {
        return FromDate.HasValue
        && ToDate.HasValue;
    }

    public override string GetSettingKey() => "OWP_EmployeeWorkingShift_DeleteWholeShifts";
}