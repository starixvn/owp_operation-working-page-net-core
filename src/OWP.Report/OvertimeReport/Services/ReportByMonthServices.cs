using OWP.Extensions;
using OWP.Report.OvertimeReport.Models;

namespace OWP.Report.OvertimeReport.Services;

public interface ReportByMonthServices
{
    Task<OvertimeByMonth> GetReportByMonth(OverTimeRequest request);
}

public class ReportByMonthServicesImp : ReportByMonthServices
{
    private readonly CommandRepository commandRepository;

    public ReportByMonthServicesImp(CommandRepository commandRepository)
    {
        this.commandRepository = commandRepository;
    }

    public async Task<OvertimeByMonth> GetReportByMonth(OverTimeRequest request)
    {
        var (users, overtimeData) = await commandRepository.GetReportByMonth(request);
        var overtimeDataList = overtimeData.ToList();
        var dataGroupedByUserId = overtimeDataList
            .GroupBy(x => x.UserId)
            .ToDictionary(x => x.Key, value => value.ToList());
        var daysInMonth = DateTime.DaysInMonth(request.Year, request.Month);

        return new OvertimeByMonth
        {
            DateColumns = BuildDateColumnInfos(request.Month, request.Year, daysInMonth),
            TraderOvertime = users.Select(user =>
                {
                    var overtimeByUserId = GetOvertimeByUserId(dataGroupedByUserId, user.UserId);
                    return new TraderOvertime
                    {
                        Location = user.Location,
                        Trader = user.Username,
                        Total = overtimeByUserId.Sum(x => x.Hour),
                        Overtime = BuildOvertimeInfo(overtimeByUserId)
                    };
                })
                .OrderBy(x => x.Location)
                .ThenBy(x => x.Trader),
            TotalHoursByDate = CalculateHoursByDate(overtimeDataList, daysInMonth)
        };
    }

    public static Dictionary<string, OverTimeInfo> BuildOvertimeInfo(List<OvertimeData> overtimeData)
    {
        return new Dictionary<string, OverTimeInfo>(overtimeData.ToDictionary(
                key => key.ReportDate.ToString("dd-MMM"),
                value => new OverTimeInfo
                {
                    IsOffDay = value.IsOffDay,
                    Hour = value.Hour
                })
            .OrderBy(x => x.Key));
    }

    public static IEnumerable<decimal> CalculateHoursByDate(List<OvertimeData> overtimeData, int daysInMonth)
    {
        for (var i = 1; i <= daysInMonth; i++)
        {
            var overtimeByDate = overtimeData.Where(x => x.ReportDate.Day == i).ToList();
            yield return overtimeByDate.Sum(x => x.Hour);
        }
    }

    public static IEnumerable<DateColumnInfo> BuildDateColumnInfos(int month, int year, int daysInMonth)
    {
        for (var i = 1; i <= daysInMonth; i++)
        {
            var date = new DateTimeOffset(year, month, i, 0, 0, 0, TimeSpan.Zero);
            yield return new DateColumnInfo
            {
                Date = date.ToString("dd-MMM"),
                IsWeekend = IsWeekend(date),
                DayOfWeek = date.ToString("ddd")
            };
        }
    }

    private static List<OvertimeData> GetOvertimeByUserId(Dictionary<int, List<OvertimeData>> dataGroup, int userId)
        => dataGroup.GetValueOrDefault(userId, []);

    private static bool IsWeekend(DateTimeOffset date)
    {
        return date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
    }
}