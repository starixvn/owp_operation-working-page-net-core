using NSubstitute;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Notification.Const;
using OWP.TicketCenter.Notification.Models;
using OWP.TicketCenter.Notification.Services;
using Xunit;

namespace OWP.TicketCenter.UnitTests.Notification.Services
{
    public class NewTicketSupportMessageDBWriterUnitTests
    {
        private readonly NewTicketSupportMessageDBWriterImp writer;

        private readonly Repository repository;

        public NewTicketSupportMessageDBWriterUnitTests()
        {
            repository = Substitute.For<Repository>();
            writer = new NewTicketSupportMessageDBWriterImp(repository);
        }

        [Fact]
        public async void WriteMessageToDB_MessageModel_ReturnSectionId()
        {
            // Arrange
            await repository
                .InsertNewTicketSupportNotification(new InsertNewTicketSupportNotificationCriteria());

            //Act
            var actual = await writer
                .Execute(new MessageTransform(), NotificationTemplateCode.NewTicket);

            // Assert
            Assert.True(actual.Length > 0);
        }
    }
}
