namespace OWP.OvertimeReport.Services
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ReportByMonthServices
    {
        Task<OvertimeByMonth> GetReportByMonth(OverTimeByMonthRequest request);
    }

    public class ReportByMonthServicesImp : ReportByMonthServices
    {
        private readonly CommandRepository commandRepository;

        public ReportByMonthServicesImp(CommandRepository commandRepository)
        {
            this.commandRepository = commandRepository;
        }

        public async Task<OvertimeByMonth> GetReportByMonth(OverTimeByMonthRequest request)
        {
            var result = await commandRepository.GetReportByMonth(request);

            return new OvertimeByMonth
            {
                DateColumns = BuildDateColumnInfos(request.Month, request.Year),
                TraderOvertime = new List<TraderOvertime>(result.GroupBy(x => new { x.Location, x.Username }, (key, values) =>
                    new TraderOvertime
                    {
                        Location = key.Location,
                        Trader = key.Username,
                        Total = values.Sum(x => x.Hour),
                        Overtime = BuildOvertimeInfo(values)
                    })
                .OrderBy(x => x.Location)
                .ThenBy(x => x.Trader))
            };
        }

        public static Dictionary<string, OverTimeInfo> BuildOvertimeInfo(IEnumerable<OvertimeData> values)
        {
            return new Dictionary<string, OverTimeInfo>(values.ToDictionary(
                key => key.ReportDate.ToString("dd-MMM"),
                value => new OverTimeInfo
                {
                    IsOffDay = value.IsOffDay,
                    Hour = value.Hour
                }).ToList()
            .OrderBy(x => x.Key));
        }

        private static bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
        }

        public IEnumerable<DateColumnInfo> BuildDateColumnInfos(int month, int year)
        {
            var daysInMonth = DateTime.DaysInMonth(2021, 10);

            for (var i = 1; i <= daysInMonth; i++)
            {
                var date = new DateTime(year, month, i);
                yield return new DateColumnInfo
                {
                    Date = date.ToString("dd-MMM"),
                    IsWeekend = IsWeekend(date),
                    DayOfWeek = date.ToString("ddd"),
                };
            }
        }
    }
}
