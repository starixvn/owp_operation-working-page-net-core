using Newtonsoft.Json.Converters;

namespace OWP.Attributes;

public class DateTimeWithoutGmtConverter : IsoDateTimeConverter
{
    public DateTimeWithoutGmtConverter()
    {
        DateTimeFormat = "yyyy-MM-dd HH:mm:ss.FFF";
    }
}