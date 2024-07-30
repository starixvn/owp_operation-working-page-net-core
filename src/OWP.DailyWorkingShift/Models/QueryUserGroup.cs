namespace OWP.DailyWorkingShift.Models
{
    using System.Collections.Generic;

    public class QueryUserGroup
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string SectionName { get; set; }

        public int ShiftId { get; set; }

        public string LevelName { get; set; }

        public int LevelId { get; set; }

        public string GroupName { get; set; }

        public string Remark { get; set; }

        public IEnumerable<EmployeeWorkingShiftNote> Notes { get; set; }

        public IEnumerable<QueryUserShift> Shifts { get; set; }

        public List<QueryUserMatchGroup> Matches { get; set; }
    }
}