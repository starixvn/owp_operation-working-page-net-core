using System.Collections.Generic;

namespace OWP.OvertimeReport.Models
{
    public class OvertimeByMonth
    {
        public string Location { get; set; }

        public string Trader { get; set; }

        public string TotalHours { get; set; }

        public List<OverTimeInfo> Overtime { get; set; }
    }

    public class OverTimeInfo
    {
        public string Date { get; set; }

        public string Hour { get; set; }

        public string DayOfWeek { get; set; }

        public bool IsWeekend { get; set; }

        public bool IsOffday { get; set; }
    }
}
