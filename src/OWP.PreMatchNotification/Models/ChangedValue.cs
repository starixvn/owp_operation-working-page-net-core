namespace OWP.PreMatchNotification.Models
{
    using System.Collections.Generic;

    public class ChangedValue
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        public int MessageTypeId { get; set; }

        public int NotificationSettingId { get; set; }

        public string NewValue { get; set; }

        public string OldValue { get; set; }

        public string MatchNo { get; set; }

        public string League { get; set; }

        public string EventName { get; set; }

        public int InfoSiteMatchId { get; set; }

        public int BetType { get; set; }

        public string BetTypeName { get; set; }

        public bool IsIndirectChangedAssignment { get; set; }

        public bool IsWatcher { get; set; }
    }

    public class AssignmentChangedValueComparer : ChangedValue, IEqualityComparer<ChangedValue>
    {
        public bool Equals(ChangedValue x, ChangedValue y)
        {
            var condition1 = x.InfoSiteMatchId == y.InfoSiteMatchId && x.BetType == y.BetType;
            var condition2 = x.MessageTypeId == y.MessageTypeId && x.UserId == y.UserId && x.IsIndirectChangedAssignment == y.IsIndirectChangedAssignment;
            var condition3 = x.OldValue == y.OldValue && x.NewValue == y.NewValue;

            return condition1 && condition2 && condition3;
        }

        public int GetHashCode(ChangedValue obj)
        {
            return obj.InfoSiteMatchId * 17 + obj.MessageTypeId * 29;
        }
    }
}