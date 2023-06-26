namespace OWP.AutoAssignment.Models.PublicApi;

public class SchedulingLAUser : LAUserInfo
{
    public bool InLineWithTV { get; set; } = true;

    public SchedulingWorkingShift[] WorkingShifts { get; set; }
}