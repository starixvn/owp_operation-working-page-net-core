
using OWP.Extensions;
using System.ComponentModel;

namespace OWP.Application.Message;
public class MessageModel
{
    public int RowNumber { get; set; }

    public int MessageId { get; set; }

    public bool IsViewed { get; set; }

    public string MessageContent { get; set; }

    public string ProfilePic { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public bool IsImportant { get; set; }

    public int MessageType { get; set; }

    public int SystemMessageType { get; set; }

    public string TimeAgo => CreatedDate.ConvertToTimeAgo();
}

public enum SystemMessageType
{
    [Description("Notification")]
    Notification = 1,

    [Description("Announcement")]
    Announcement = 2,

    [Description("Message")]
    Message = 3
}