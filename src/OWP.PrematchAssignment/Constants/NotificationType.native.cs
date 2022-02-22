namespace OWP.PreMatchAssignment.Constants
{
    using OWP.Extensions;

    public static class NotificationType
    {
        private static readonly Dictionary<string, int> NotificationTypeMap = new Dictionary<string, int>
        {
            { "PreMatch", (int)NotificationTemplateCode.PreMatchIssue },
            { "EventChecking", (int)NotificationTemplateCode.EventCheckingResponse }
        };

        public static int GetNotificationType(string page)
        {
            return NotificationTypeMap.GetValue(page, 0);
        }
    }
}