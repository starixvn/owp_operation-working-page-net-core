namespace OWP.TicketCenter.Criteria
{
    using System;
    using System.Globalization;
    using Fanex.Data.Repository;

    public abstract class InsertNotificationMessageCriteria : NonQueryCommand
    {
        public string NotificationContent { get; set; }

        public bool IsImportant { get; set; }

        public bool IsWatcher { get; set; }

        public int NotificationType { get; set; }

        public int TicketId { get; set; }

        public int ActorId { get; set; }

        public string SessionId => Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

        public override bool IsValid()
            => NotificationContent.Length > 0 &&
               TicketId > 0;
    }
}
