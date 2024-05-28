namespace OWP.TicketCenter.ViewModels;

public class MessageDetailViewModel
{
    public int TicketId { get; set; }

    public int MessageId { get; set; }

    public string Message { get; set; }

    public int UserId { get; set; }

    public string Username { get; set; }

    public string AvatarFileName { get; set; }

    public string Extension { get; set; }

    public int? FileId { get; set; }

    public bool IsSaved { get; set; }

    public bool IsFile => FileId.HasValue;

    public string ImageFileName { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string QuoteMessage { get; set; }

    public string QuoteUsername { get; set; }

    public int QuoteMessageId { get; set; }

    public DateTime QuoteCreatedDate { get; set; }

    public IList<MessageReactIcon> MessageReactIcons { get; set; }
}

public class MessageReactIcon
{
    public int MessageId { get; set; }

    public string IconKey { get; set; }

    public string ReactUserId { get; set; }

    public string ReactUserName { get; set; }

    public string ReactAvatarName { get; set; }
}