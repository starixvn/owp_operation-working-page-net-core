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
    public class NewMessageDBWriterUnitTests
    {
        private readonly NewMessageDBWriterImp writer;
        private readonly Repository repository;

        public NewMessageDBWriterUnitTests()
        {
            repository = Substitute.For<Repository>();
            writer = new NewMessageDBWriterImp(repository);
        }

        [Fact]
        public async void WriteMessageToDB_NewMessageRequest_ReturnSectionId()
        {
            // Arrange
            await repository.InsertNewMessageNotification(new InsertNewMessageNotificationCriteria());

            //Act
            var actual = await writer
                .Execute(new NewMessageRequest());

            // Assert
            Assert.True(actual.Length > 0);
        }
    }
}