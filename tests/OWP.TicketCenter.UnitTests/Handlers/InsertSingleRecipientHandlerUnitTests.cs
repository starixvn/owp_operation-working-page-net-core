namespace OWP.TicketCenter.UnitTests.Handlers;

using Models;
using Notification.Services;
using NSubstitute;
using RabbitMQ;
using TicketCenter.Handlers;
using TicketCenter.Services;
using Xunit;

public class InsertSingleRecipientHandlerUnitTests
{
    private readonly Repository repository;

    private readonly RecipientService recipientService;

    private readonly NotificationService notificationService;

    private readonly MessageClient messageClient;

    public InsertSingleRecipientHandlerUnitTests()
    {
        repository = Substitute.For<Repository>();
        recipientService = Substitute.For<RecipientService>();
        notificationService = Substitute.For<NotificationService>();
        messageClient = Substitute.For<MessageClient>();
    }

    [Fact]
    public async void InsertSingleRecipient_RecipientInfo_ReturnIdFromDB()
    {
        // Arrange
        var handler = new InsertRecipientHandler(
            repository,
            recipientService,
            notificationService,
            messageClient
        );

        var recipients = new List<InsertOrRemoveManyRecipientModel>();

        recipientService
            .InsertManyRecipientReader(recipients, 1, 3)
            .Returns(x => true);

        repository
            .AddSingleRecipient(Arg.Compat.Any<InsertSingleRecipientRequest>())
            .ReturnsForAnyArgs(x => 1);

        messageClient.SendMessage(Arg.Compat.Any<object>(), Arg.Compat.Any<string>());

        var actual = await handler.Handle(new InsertSingleRecipientRequest(), CancellationToken.None);

        //Assert
        var expected = 1;
        Assert.Equal(actual, expected);
    }
}