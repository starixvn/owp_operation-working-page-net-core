using System.Collections.Generic;

namespace OWP.OtherSport.Notification.Models
{
    public class Message
    {
        public bool IsImportant { get; set; }

        public int MessageType { get; set; }

        public string MessageContent { get; set; }

        public List<string> ReceivedUserIdList { get; set; }
    }
}
