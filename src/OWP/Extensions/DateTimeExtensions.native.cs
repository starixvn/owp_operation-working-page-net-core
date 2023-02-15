namespace OWP.Extensions;

using System.Globalization;

public static class DateTimeExtensions
{
    private const int TimeZoneGMT8 = 8;
    private const double HalfOfDateSubtract = 0.5;

    public static string ConvertToTimeAgo(this DateTime date)
    {
        var timeSpan = DateTime.Now - date;

        if (timeSpan.Days != 0)
        {
            return timeSpan.Days + " days ago";
        }

        if (timeSpan.Hours != 0)
        {
            return timeSpan.Hours + " hours ago";
        }

        if (timeSpan.Minutes != 0)
        {
            return timeSpan.Minutes + " minutes ago";
        }

        return timeSpan.Seconds + " seconds ago";
    }

    public static string ToKOTime(this TimeSpan koTime)
    {
        var parsedTime = new DateTime(1, 1, 1, koTime.Hours, koTime.Minutes, 0);

        return parsedTime.ToString("hh:mm tt", CultureInfo.InvariantCulture);
    }

    public static DateTime ToGmt8(this DateTime dateTime)
    {
        return dateTime.ToUniversalTime().AddHours(TimeZoneGMT8);
    }

    public static DateTime StartOfDate(this DateTime dateTime)
    {
        return dateTime.Date;
    }

    public static DateTime StartHalfOfDate(this DateTime dateTime)
    {
        return dateTime.Date.AddDays(HalfOfDateSubtract).AddSeconds(-1);
    }

    public static DateTime TimeRangeOfDate(this DateTime dateTime)
    {
        var startOfDay = dateTime.Date.StartOfDate();

        return startOfDay.AddDays(1).StartHalfOfDate();
    }
}