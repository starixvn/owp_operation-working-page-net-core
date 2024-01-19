namespace OWP.Event.LiveGameAssignment.Download.Converter;

public static class DateTimeConverter
{
    const int TimeZoneGMT8 = 8;
    public static DateTime ConvertTimeToUTC8(DateTime date)
        => date.AddHours(TimeZoneGMT8);
}
