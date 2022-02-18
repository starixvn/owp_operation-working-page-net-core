

namespace OWP.OvertimeReport.Models
{
    public class OvertimeByMonth
    {
        public IEnumerable<DateColumnInfo> DateColumns { get; set; }

        public IEnumerable<TraderOvertime> TraderOvertime { get; set; }

        public IEnumerable<decimal> TotalHoursByDate { get; set; }

        public decimal TotalHours
        {
            get => TraderOvertime.Sum(x => x.Total);
        }
    }

    public class TraderOvertime
    {
        public string Location { get; set; }

        public string Trader { get; set; }

        public decimal Total { get; set; }

        public Dictionary<string, OverTimeInfo> Overtime { get; set; }
    }

    public class OverTimeInfo
    {
        public decimal Hour { get; set; }

        public bool IsOffDay { get; set; }
    }

    public class DateColumnInfo
    {
        public string Date { get; set; }

        public bool IsWeekend { get; set; }

        public string DayOfWeek { get; set; }
    }
}
