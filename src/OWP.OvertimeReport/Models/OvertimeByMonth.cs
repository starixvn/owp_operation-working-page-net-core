using System.Collections.Generic;
using System.Linq;

namespace OWP.OvertimeReport.Models
{
    public class OvertimeByMonth
    {
        public List<TraderOvertime> TraderOvertime { get; set; }

        public decimal TotalHours
        {
            get => TraderOvertime.Sum(x => x.TotalHours);
        }
    }

    public class TraderOvertime
    {
        public string Location { get; set; }

        public string Trader { get; set; }

        public decimal TotalHours { get; set; }

        public Dictionary<string, OverTimeInfo> Overtime { get; set; }
    }

    public class OverTimeInfo
    {
        public decimal Hour { get; set; }

        public string DayOfWeek { get; set; }

        public bool IsWeekend { get; set; }

        public bool IsOffDay { get; set; }
    }

    public class OverTimeByMonthRequest
    {
        public string Location { get; set; }

        public string Trader { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }
    }
}
