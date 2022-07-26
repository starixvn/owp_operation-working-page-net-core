using NSubstitute;
using OWP.TicketCenter.Handlers;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Notification.Services;
using OWP.TicketCenter.Services;

using Xunit;

namespace OWP.TicketCenter.UnitTests.Handlers
{
    public class InsertSingleRecipientHandlerUnitTests
    {
        private readonly Repository repository;

        private readonly RecipientService recipientService;

        private readonly NotificationService notificationService;


        public InsertSingleRecipientHandlerUnitTests()
        {
            repository = Substitute.For<Repository>();
            recipientService = Substitute.For<RecipientService>();
            notificationService = Substitute.For<NotificationService>();
        }

        [Fact]
        public async void InsertSingleRecipient_RecipientInfo_ReturnIdFromDB()
        {
            // Arrange
            var handler = new InsertRecipientHandler(
                repository,
                recipientService,
                notificationService,
                null
                );

            var recipients = new List<InsertOrRemoveManyRecipientModel>();

            recipientService
                .InsertManyRecipientReader(recipients, 1, 3)
                .Returns(x => true);

            repository
                .AddSingleRecipient(Arg.Compat.Any<InsertSingleRecipientRequest>())
                .ReturnsForAnyArgs(x => 1);

            var actual = await handler.Handle(new InsertSingleRecipientRequest(), CancellationToken.None);

            //Assert
            var expected = 1;
            Assert.Equal(actual, expected);
        }
    }
}