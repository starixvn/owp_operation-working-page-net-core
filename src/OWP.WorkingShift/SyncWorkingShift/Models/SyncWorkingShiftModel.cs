namespace OWP.WorkingShift.SyncWorkingShift.Models;

[Serializable]
public class SyncWorkingShiftModel
{
    public string EmployeeID { get; set; }
    public DateTime? ShiftDate { get; set; }
    public string ShiftCode { get; set; }
    public DateTime? ShiftStartTime { get; set; }
    public DateTime? ShiftEndTime { get; set; }
    public DateTime? LastUpdated { get; set; }

    public bool IsOffDay { get; set; }
    public bool IsPublicHoliday { get; set; }
}