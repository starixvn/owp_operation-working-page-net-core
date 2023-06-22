using NSubstitute;
using OWP.RabbitMQ;
using OWP.TicketCenter.Handlers;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Notification.Services;
using OWP.TicketCenter.Services;
using Xunit;

namespace OWP.TicketCenter.UnitTests.Handlers;

public class InsertSingleRecipientHandlerUnitTests
{
    private readonly Repository repository;

    private readonly RecipientService recipientService;

    private readonly MessageClient messageClient;

    private readonly InsertRecipientHandler handler;

    public InsertSingleRecipientHandlerUnitTests()
    {
        repository = Substitute.For<Repository>();
        recipientService = Substitute.For<RecipientService>();
        var notificationService = Substitute.For<NotificationService>();
        messageClient = Substitute.For<MessageClient>();
        var preMatchNotificationService = Substitute.For<PreMatchNotificationService>();

        handler = new InsertRecipientHandler(
            repository,
            recipientService,
            notificationService,
            messageClient, preMatchNotificationService
        );
    }

    [Fact]
    public async Task InsertSingleRecipient_RecipientInfo_ReturnIdFromDB()
    {
        var recipients = new List<InsertOrRemoveManyRecipientModel>();

        recipientService
            .InsertManyRecipientReader(recipients, 1, 3)
            .Returns(_ => true);

        repository
            .AddSingleRecipient(Arg.Compat.Any<InsertSingleRecipientRequest>())
            .ReturnsForAnyArgs(_ => 1);

        messageClient.SendMessage(Arg.Compat.Any<object>(), Arg.Compat.Any<string>());

        var actual = await handler.Handle(new InsertSingleRecipientRequest(), CancellationToken.None);

        //Assert
        const int expected = 1;
        Assert.Equal(expected, actual);
    }
}