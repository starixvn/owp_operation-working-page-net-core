namespace OWP.WorkingShift.WeeklyWorkingShift.Models;

using System;
using System.Collections.Generic;
    

public class WorkingShiftGroupByDateModel
{
    public WorkingShiftColorModel WorkingShiftColor { get; set; }

    public Dictionary<DateTime, List<Common.Models.WorkingShiftModel>> WorkingShiftDates { get; set; }
}