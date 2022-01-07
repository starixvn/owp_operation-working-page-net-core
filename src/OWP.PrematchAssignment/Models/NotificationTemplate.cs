namespace OWP.PreMatchAssignment.Models
{
    public class NotificationTemplate
    {
        public string NotificationContent { get; set; }

        public bool IsImportant { get; set; }

        public bool IsWatcher { get; set; }

        public int NotificationType { get; set; }

        public int PreMatchIssueId { get; set; }
    }
}