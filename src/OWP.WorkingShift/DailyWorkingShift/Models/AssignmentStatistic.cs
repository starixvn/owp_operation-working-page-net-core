namespace OWP.WorkingShift.DailyWorkingShift.Models
{
    using System.Collections.Generic;

    public class AssignmentStatistic
    {
        public IList<int> MatchAssigned { get; set; } = new List<int>();

        public IList<int> TraderAssigned { get; set; } = new List<int>();

        public IList<int> LAAssigned { get; set; } = new List<int>();

        public IList<int> MatchSelected { get; set; } = new List<int>();
    }
}