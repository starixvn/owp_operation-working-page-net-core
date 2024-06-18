namespace OWP.WorkingShift.Common.Models;

using System;

public class CopySingleShiftModel
{
    public int UserId { get; set; }

    public DateTime ShiftDate { get; set; }

    public int ShiftId { get; set; }

    public int? RefShiftId { get; set; }
}