namespace OWP.NotificationHandler.Models;

public class MessageModel
{
    public int UserId { get; set; }

    public bool IsImportant { get; set; }

    public int MessageType { get; set; }

    public string MessageContent { get; set; }
}


public class NotificationModel : MessageModel
{
    public int TicketId { get; set; }

    public int TicketType { get; set; }

    public int EventType { get; set; }

    public int? TotalMessageUnread { get; set; }

    public bool IsNewMessage { get; set; }
}


public class TicketCenterContent
{
    public string NotificationId { get; set; }
    public NotificationModel Message { get; set; }
}
