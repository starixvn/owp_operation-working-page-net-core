using OWP.Models;

namespace OWP.Extensions;

public static class TimeRangeExtensions
{
    public static TimeRangeResult CheckMainMatch(DateTime eventDate)
    {
        return CheckMatch(eventDate);
    }

    public static TimeRangeResult CheckMatch(DateTime eventDate)
    {
        var dateTime = DateTime.Now.ToGmt8();
        var timeRange = dateTime.TimeRangeOfDate();
        return CheckMatch(timeRange, eventDate);
    }

    public static TimeRangeResult CheckMatch(this DateTime toTime, DateTime eventDate)
    {
        var isMatchInTimeRange = eventDate <= toTime;
        return new TimeRangeResult
        {
            IsMatchInTimeRange = isMatchInTimeRange,
            IsAllowedToViewDetail = isMatchInTimeRange
        };
    }
}