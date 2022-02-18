namespace OWP.PreMatchNotification.Services
{
    

    public interface DateTimeHelper
    {
        DateTime GetCurrentDate();
    }

    public class DateTimeHelperImp : DateTimeHelper
    {
        public DateTime GetCurrentDate()
            => DateTime.Today;
    }
}