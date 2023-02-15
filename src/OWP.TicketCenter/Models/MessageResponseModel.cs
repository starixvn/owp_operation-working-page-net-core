namespace OWP.TicketCenter.Models;

public class MessageResponseModel
{
    public int UserId { get; set; }

    public string Username { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string Message { get; set; }

    public int MessageId { get; set; }

    public bool IsFile => FileId.HasValue;

    public int? FileId { get; set; }
}

public class MessageInfoModel : MessageResponseModel
{
    public int TicketId { get; set; }
}