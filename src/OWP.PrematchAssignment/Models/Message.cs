using System;

namespace OWP.PreMatchAssignment.Models
{
    public class Message
    {
        public int PreMatchIssueId { get; set; }

        public string MessageContent { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Nickname { get; set; }

        public bool IsMyMessage { get; set; }

        public int? FileId { get; set; }

        public bool IsFileAvailable { get; set; }

        public bool IsSaved { get; set; }

        public int PreMatchIssueMessageId { get; set; }
    }
}
