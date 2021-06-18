namespace OWP.PreMatchNotification.Models
{
    using System.Collections.Generic;

    public class ChangedValue
    {
        public List<int> ReceivedUserIds { get; set; }

        public int MessageTypeId { get; set; }

        public string NewValue { get; set; }

        public string OldValue { get; set; }

        public string League { get; set; }

        public string Home { get; set; }

        public string Away { get; set; }

        public int InfoSiteMatchId { get; set; }

        public string BetTypeName { get; set; }

        public string KOTime { get; set; }

        public bool IsIndirectChangedAssignment { get; set; }
    }
}