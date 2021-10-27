using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Notification.Const;
using OWP.TicketCenter.Notification.Models;
using OWP.TicketCenter.Notification.Services;
using Xunit;

namespace OWP.TicketCenter.UnitTests.Notification.Services
{
    public class NewTicketEventMessageDBWriterUnitTests
    {
        private readonly NewTicketMatchMessageDBWriterImp writer;

        private readonly Repository repository;

        public NewTicketEventMessageDBWriterUnitTests()
        {
            repository = Substitute.For<Repository>();
            writer = new NewTicketMatchMessageDBWriterImp(repository);
        }

        [Fact]
        public async void WriteMessageToDB_MessageModel_ReturnSectionId()
        {
            // Arrange
            await repository
                .InsertNewTicketMatchNotification(new InsertNewTicketMatchNotificationCriteria());

            //Act
            var actual = await writer
                .Execute(new MessageTransform(), NotificationTemplateCode.NewTicket);

            // Assert
            Assert.True(actual.Length > 0);
        }
    }
}
