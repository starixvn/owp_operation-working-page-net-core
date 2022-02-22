namespace OWP.Utils
{
    public static class DateTimeExtensions
    {
        private const int TIME_ZONE_GMT8 = 8;

        public static DateTime ToGmt8(this DateTime dateTime)
            => dateTime.ToUniversalTime().AddHours(TIME_ZONE_GMT8);
    }
}