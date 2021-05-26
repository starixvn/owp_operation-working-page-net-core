namespace OWP.OtherSport.Notification.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using OWP.OtherSport.Notification.Extensions;
    using OWP.OtherSport.Notification.Models;

    public interface AssigneeChangedValueFilter
    {
        Task<List<ChangedValue>> Filter(List<ChangedValue> changedValues);
    }

    public class AssigneeMessageFilterImp : AssigneeChangedValueFilter
    {
        private readonly NotificationSettingQueryRepository repository;

        public AssigneeMessageFilterImp(NotificationSettingQueryRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<ChangedValue>> Filter(List<ChangedValue> changedValues)
        {
            changedValues.ForEach(c => c.NotificationSettingId = NotificationSettingHelper.GetNotificationSettingIdForAssignee(c.MessageTypeId, c.IsIndirectChangedAssignment));

            var unsubcribeSettings = await repository.GetUnsubscribeSettings();
            return changedValues.Where(m => !unsubcribeSettings.Contains($"{m.UserId}-{m.NotificationSettingId}")).ToList();
        }
    }
}