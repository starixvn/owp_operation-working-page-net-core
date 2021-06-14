namespace OWP.PreMatchNotification.Extensions
{
    using System.Collections.Generic;
    using OWP.PreMatchNotification.Models;

    public static class AssigneeMessageFilter
    {
        public static List<ChangedValue> Filter(List<ChangedValue> changedValues)
            => changedValues.ForEach(c => c.NotificationSettingId = NotificationSettingHelper.GetNotificationSettingIdForAssignee(c.MessageTypeId, c.IsIndirectChangedAssignment)).ToList();
    }
}