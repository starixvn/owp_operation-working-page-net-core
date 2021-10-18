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
                TraderOvertime = new List<TraderOvertime>(result.GroupBy(x => new { x.Location, x.Username }, (key, values) =>
                    new TraderOvertime
                    {
                        Location = key.Location,
                        Trader = key.Username,
                        TotalHours = values.Sum(x => x.Hour),
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
                    IsWeekend = IsWeekend(value.ReportDate),
                    DayOfWeek = value.ReportDate.ToString("ddd"),
                    Hour = value.Hour
                }).ToList()
            .OrderBy(x => x.Key));
        }

        private static bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
        }
    }
}
