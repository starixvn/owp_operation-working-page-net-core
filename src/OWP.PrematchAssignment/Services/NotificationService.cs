namespace OWP.PreMatchAssignment.Services
{
    
    using System.Collections.Immutable;
        
    using Constants;
    using Criteria;
    using Fanex.Data.Repository;
    using Fanex.NAP.Core.Web.Session;
    using Models;

    public interface NotificationService
    {
        Task<string> Notify(Models.Message message, int? fileId, bool isPreMatch = true);
    }

    public class NotificationServiceImp : NotificationService
    {
        #region MessageTemplate

        private static readonly ImmutableList<NotificationTemplate> notificationTemplates = ImmutableList<NotificationTemplate>.Empty.AddRange(new[]
        {
            new NotificationTemplate
            {
                NotificationType = (int)NotificationTemplateCode.PreMatchIssue,
                NotificationContent = "New Issue in <League Name>/<Event Name>: <issue> by <username>",
                IsImportant = true,
                IsWatcher = false
            },

            new NotificationTemplate
            {
                NotificationType = (int)NotificationTemplateCode.EventCheckingResponse,
                NotificationContent = "New Response for Issue in <League Name>/<Event Name>: <issue> by <username>",
                IsImportant = true,
                IsWatcher = false
            },

            new NotificationTemplate
            {
                NotificationType = (int)NotificationTemplateCode.Attachment,
                NotificationContent = "New attachment in <League Name>/<Event Name> by <username>",
                IsImportant = true,
                IsWatcher = false
            }
        });

        #endregion MessageTemplate

        private static readonly Dictionary<int, NotificationTemplate> notificationTemplateMap = notificationTemplates.ToDictionary(m => m.NotificationType, m => m);

        private readonly ICurrentSession currentSession;

        private readonly IDynamicRepository dynamicRepo;

        public NotificationServiceImp(ICurrentSession currentSession, IDynamicRepository dynamicRepo)
        {
            this.currentSession = currentSession;
            this.dynamicRepo = dynamicRepo;
        }

        public async Task<string> Notify(Models.Message message, int? fileId, bool isPreMatch = true)
        {
            var actorId = currentSession.UserId;
            var notificationTemplate = notificationTemplateMap[GetNotificationType(fileId, isPreMatch)];
            notificationTemplate.PreMatchIssueId = message.PreMatchIssueId;

            var criteria = new InsertNotificationMessageCriteria
            {
                ActorId = actorId,
                IsImportant = notificationTemplate.IsImportant,
                IsWatcher = notificationTemplate.IsWatcher,
                NotificationContent = notificationTemplate.NotificationContent.Replace("<issue>", message.MessageContent),
                NotificationType = notificationTemplate.NotificationType,
                PreMatchIssueId = message.PreMatchIssueId
            };

            await dynamicRepo.ExecuteAsync(criteria);

            return criteria.SessionId;
        }

        private static int GetNotificationType(int? fileId, bool IsPreMatch)
        {
            if (fileId.HasValue)
            {
                return (int)NotificationTemplateCode.Attachment;
            }

            return !IsPreMatch ? (int)NotificationTemplateCode.EventCheckingResponse : (int)NotificationTemplateCode.PreMatchIssue;
        }
    }
}