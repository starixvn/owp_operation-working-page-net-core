namespace OWP.RabbitMQ.Model;

public class TicketMessageModel
{
    public int UserId { get; set; }

    public bool IsImportant { get; set; }

    public int MessageType { get; set; }

    public string MessageContent { get; set; }
}


public class NotificationModel : TicketMessageModel
{
    public int TicketId { get; set; }

    public int TicketType { get; set; }

    public int EventType { get; set; }

    public int? TotalMessageUnread { get; set; }

    public bool IsNewMessage { get; set; }
}
