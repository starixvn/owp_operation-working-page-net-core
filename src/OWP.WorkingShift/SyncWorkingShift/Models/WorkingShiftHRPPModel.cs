namespace OWP.WorkingShift.SyncWorkingShift.Models;

public class WorkingShiftHRPPModel
{
    public string EmployeeID { get; set; }

    public DateTime? ScheduleDate { get; set; }

    public DateTime? ShiftStartTime { get; set; }

    public DateTime? ShiftEndTime { get; set; }

    public DateTime? LastUpdated { get; set; }

    public bool IsDayOff { get; set; }

    public bool IsPublicHoliday { get; set; }
}