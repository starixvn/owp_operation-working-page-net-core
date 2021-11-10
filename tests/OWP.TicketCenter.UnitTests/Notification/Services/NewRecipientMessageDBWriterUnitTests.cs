using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Handlers;
using OWP.TicketCenter.Notification.Const;
using OWP.TicketCenter.Notification.Models;
using OWP.TicketCenter.Notification.Services;
using Xunit;

namespace OWP.TicketCenter.UnitTests.Notification.Services
{
    public class NewRecipientMessageDBWriterUnitTests
    {
        private readonly NewRecipientMessageDBWriterImp writer;
        private readonly Repository repository;

        public NewRecipientMessageDBWriterUnitTests()
        {
            repository = Substitute.For<Repository>();
            writer = new NewRecipientMessageDBWriterImp(repository);
        }

        [Fact]
        public async void WriteMessageToDB_InsertSingleRecipientRequest_ReturnSectionId()
        {
            // Arrange
            await repository
                .InsertNewRecipientNotification(new InsertNewRecipientNotificationCriteria());

            //Act
            var actual = await writer
                .Execute(new InsertSingleRecipientRequest());

            // Assert
            Assert.True(actual.Length > 0);
        }
    }
}