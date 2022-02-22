namespace OWP.PreMatchAssignment.UnitTests.Services
{
    
    using Constants;
    using Criteria;
    using Fanex.Data.Repository;
    using Fanex.NAP.Core.Web.Session;
    using Models;
    using NSubstitute;
    using PreMatchAssignment.Services;
    using Xunit;

    public class NotificationServiceUnitTests
    {
        [Fact]
        public async Task Notify_ActorInESTSection_ShouldReturnESTResponseNotification()
        {
            //Arrange
            var dynamicRepo = Substitute.For<IDynamicRepository>();
            var currentSession = Substitute.For<ICurrentSession>();
            currentSession.UserId = 8;
            var notiService = new NotificationServiceImp(currentSession, dynamicRepo);

            //Act
            await notiService.Notify(new Message { PreMatchIssueId = 1, MessageContent = "abc" }, null, false);

            //Assert
            await dynamicRepo.Received().ExecuteAsync(Arg.Compat.Is<InsertNotificationMessageCriteria>(p =>
                p.ActorId == 8 &&
                p.IsImportant &&
                !p.IsWatcher &&
                p.PreMatchIssueId == 1 &&
                p.NotificationType == (int)NotificationTemplateCode.EventCheckingResponse &&
                p.NotificationContent == "New Response for Issue in <League Name>/<Event Name>: abc by <username>"));
        }

        [Fact]
        public async Task Notify_ActorNotInESTSection_ShouldReturnNewIssueNotification()
        {
            //Arrange
            var dynamicRepo = Substitute.For<IDynamicRepository>();
            var currentSession = Substitute.For<ICurrentSession>();
            currentSession.UserId = 8;
            var notiService = new NotificationServiceImp(currentSession, dynamicRepo);

            //Act
            await notiService.Notify(new Message { PreMatchIssueId = 1, MessageContent = "abc" }, null, true);

            //Assert
            await dynamicRepo.Received().ExecuteAsync(Arg.Compat.Is<InsertNotificationMessageCriteria>(p =>
                p.ActorId == 8 &&
                p.IsImportant &&
                !p.IsWatcher &&
                p.PreMatchIssueId == 1 &&
                p.NotificationType == (int)NotificationTemplateCode.PreMatchIssue &&
                p.NotificationContent == "New Issue in <League Name>/<Event Name>: abc by <username>"));
        }

        [Fact]
        public async Task Notify_AttachFile_ShouldReturnNewAttachmentNotification()
        {
            //Arrange
            var dynamicRepo = Substitute.For<IDynamicRepository>();
            var currentSession = Substitute.For<ICurrentSession>();
            currentSession.UserId = 8;
            var notiService = new NotificationServiceImp(currentSession, dynamicRepo);

            //Act
            await notiService.Notify(new Message { PreMatchIssueId = 1, MessageContent = "abc" }, 1, true);

            //Assert
            await dynamicRepo.Received().ExecuteAsync(Arg.Compat.Is<InsertNotificationMessageCriteria>(p =>
                p.ActorId == 8 &&
                p.IsImportant &&
                !p.IsWatcher &&
                p.PreMatchIssueId == 1 &&
                p.NotificationType == (int)NotificationTemplateCode.Attachment &&
                p.NotificationContent == "New attachment in <League Name>/<Event Name> by <username>"));
        }
    }
}