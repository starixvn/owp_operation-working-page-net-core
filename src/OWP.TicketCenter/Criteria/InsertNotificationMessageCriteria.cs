namespace OWP.TicketCenter.Criteria
{
    using System;
    using System.Globalization;
    using Fanex.Data.Repository;

    public class InsertNotificationMessageCriteria : NonQueryCommand
    {
        public string NotificationContent { get; set; }

        public bool IsImportant { get; set; }

        public bool IsWatcher { get; set; }

        public int NotificationType { get; set; }

        public int PreMatchIssueId { get; set; }

        public int ActorId { get; set; }

        public string SessionId => Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

        public override string GetSettingKey() => "OWP_TicketCenter_InsertNewMessageNotification";

        public override bool IsValid() => true;
    }
}
