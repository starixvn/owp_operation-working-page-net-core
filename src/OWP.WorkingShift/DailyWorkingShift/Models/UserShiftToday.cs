namespace OWP.WorkingShift.DailyWorkingShift.Models
{
    public class UserShiftToday : QueryUserShift
    {
        public int LevelId { get; set; }

        public string UserName { get; set; }
    }
}