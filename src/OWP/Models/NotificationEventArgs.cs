namespace OWP.Models;

public class NotificationEventArgs
{
    public string UserId { get; set; }

    public IList<Message> Messages { get; set; }
}