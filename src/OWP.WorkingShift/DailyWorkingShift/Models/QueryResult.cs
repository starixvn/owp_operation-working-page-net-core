namespace OWP.WorkingShift.DailyWorkingShift.Models
{
    using System.Collections.Generic;

    public class QueryResult
    {
        public IEnumerable<QueryUserGroup> Users { get; set; }

        public IEnumerable<QueryMatchInfo> Matches { get; set; }

        public AssignmentStatistic AssignmentStatistics { get; set; }
    }
}