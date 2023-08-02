namespace OWP.TicketCenter.ViewModels;

public class MessageDetailViewModel
{
    public int MessageId { get; set; }

    public string Message { get; set; }

    public int UserId { get; set; }

    public string Username { get; set; }

    public int? FileId { get; set; }

    public bool IsSaved { get; set; }

    public bool IsFile => FileId.HasValue;

    public byte[] ImageContent { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string QuoteMessage { get; set; }

    public string QuoteUsername { get; set; }

    public DateTime QuoteCreatedDate { get; set; }
}