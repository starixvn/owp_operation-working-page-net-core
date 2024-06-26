namespace OWP.Models;
public class NotificationEventArgs
{
    public int UserId { get; set; }

    public IList<Message> Messages { get; set; }
}