namespace OWP.Attributes;

using Newtonsoft.Json.Converters;

public class DateTimeWithoutGmtConverter : IsoDateTimeConverter
{
    public DateTimeWithoutGmtConverter()
    {
        DateTimeFormat = "yyyy-MM-dd HH:mm:ss.FFF";
    }
}
