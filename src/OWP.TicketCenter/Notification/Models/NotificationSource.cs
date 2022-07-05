namespace OWP.TicketCenter.Notification.Models;

public class NotificationSource
{
    public string SessionId { get; set; }

    public int TicketId { get; set; }

    public int EventType { get; set; }

    public bool IsNewMessage { get; set; }

    public NotificationSource()
    {

    }
    public NotificationSource(string sessionId, int ticketId, int eventType, bool isNewMessage = false)
    {
        SessionId = sessionId;
        TicketId = ticketId;
        EventType = eventType;
        IsNewMessage = isNewMessage;
    }
}
