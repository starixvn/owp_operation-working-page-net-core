namespace OWP.PreMatchNotification.Services
{
    using System;

    public interface DateTimeHelper
    {
        DateTime GetCurrentDate();
    }

    public class DateTimeHelperImp : DateTimeHelper
    {
        public DateTime GetCurrentDate()
            => DateTime.Now.AddHours(12).Date;
    }
}