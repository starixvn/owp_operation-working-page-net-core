namespace OWP.WorkingShift.WeeklyWorkingShift.Models;

using System;

public class QueryModel
{
    public string User { get; set; }

    public string Section { get; set; }

    public string DepartmentId { get; set; }

    public string Category { get; set; }

    public string Location { get; set; }

    public bool ExcludeClosedAccount { get; set; } = true;

    public int? OrderUserId { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public bool IsFrame { get; set; }

    public int TimeZoneOffset { get; set; }

    public WorkingShiftPermission Permission { get; set; }
}