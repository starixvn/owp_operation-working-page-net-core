using System;

namespace OWP.Utils
{
    public static class DateTimeExtensions
    {
        private const int TIME_ZONE_GMT7 = 8;

        public static DateTime ToGmt7(this DateTime dateTime)
        {
            return dateTime.ToUniversalTime().AddHours(TIME_ZONE_GMT7);
        }
    }
}
