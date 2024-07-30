namespace OWP.WorkingShift.Common.Criteria;

using System;
using Fanex.Data.Repository;

public class InsertWorkingShiftColorCriteria : NonQueryCommand
{
    public int ActorId { get; set; }

    public string ShiftCode { get; set; }

    public string Background { get; set; }

    public string Color { get; set; }

    public TimeSpan? StartTime { get; set; }

    public TimeSpan? EndTime { get; set; }

    public int? Extension { get; set; }

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public int ShiftId { get; set; }

    public override bool IsValid() => !string.IsNullOrWhiteSpace(Background) && !string.IsNullOrWhiteSpace(Color);

    public override string GetSettingKey() => "OWP_EmployeeWorkingShift_QuickInsert_WorkingShiftColor";
}