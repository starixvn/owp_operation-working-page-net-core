
using OWP.Extensions;

namespace OWP.Application.Message;
public class MessageModel
{
    public int RowNumber { get; set; }

    public int MessageId { get; set; }

    public bool IsViewed { get; set; }

    public string MessageContent { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public bool IsImportant { get; set; }

    public int MessageType { get; set; }

    public string TimeAgo => CreatedDate.ConvertToTimeAgo();
}
