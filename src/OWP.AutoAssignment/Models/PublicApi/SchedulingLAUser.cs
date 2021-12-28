namespace OWP.AutoAssignment.Models.PublicApi
{
    public class SchedulingLAUser : LAUserInfo
    {
        public bool InLineWithTV { get; } = true;

        public SchedulingWorkingShift[] WorkingShifts { get; set; }
    }
}