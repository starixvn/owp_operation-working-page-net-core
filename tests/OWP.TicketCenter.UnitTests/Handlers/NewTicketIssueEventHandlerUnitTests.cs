using AutoMapper;
using Fanex.Data.Repository;
using Fanex.NAP.Client.Helpers.Web;
using NSubstitute;
using OWP.RabbitMQ;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Handlers;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Notification.Services;
using OWP.TicketCenter.Services;
using Xunit;

namespace OWP.TicketCenter.UnitTests.Handlers;

public class NewTicketIssueEventHandlerUnitTests
{
    private readonly Repository repository;

    private readonly MessageService messageService;

    private readonly IMapper mapper;

    private readonly NotificationService notificationService;

    private readonly RecipientService recipientService;

    private readonly NewTicketIssueEventHandler handler;


    private readonly PreMatchNotificationService preMatchNotificationService;

    public NewTicketIssueEventHandlerUnitTests()
    {
        repository = Substitute.For<Repository>();
        messageService = Substitute.For<MessageService>();
        mapper = Substitute.For<IMapper>();
        notificationService = Substitute.For<NotificationService>();
        recipientService = Substitute.For<RecipientService>();
        var messageClient = Substitute.For<MessageClient>();


        preMatchNotificationService = Substitute.For<PreMatchNotificationService>();

        handler = new NewTicketIssueEventHandler(
            repository,
            messageService,
            notificationService,
            recipientService,
            messageClient,
            preMatchNotificationService,
            mapper
        );
    }

    [Fact]
    public async Task InsertNewEventIssue_IssueInfo_ReturnTicketId()
    {
        // Arrange
        var request = new NewTicketIssueEventRequest();

        repository
            .CreateTicketIssue(Arg.Compat.Any<CriteriaBase>())
            .Returns(Task.FromResult(111));

        await repository.AddNewMessage(Arg.Compat.Any<NewMessageRequestModel>());

        await repository.InsertIssueCreator(Arg.Compat.Any<InsertIssueCreatorCriteria>());

        recipientService.InsertManyRecipient(
                Arg.Compat.Any<IList<InsertOrRemoveManyRecipientModel>>(),
                Arg.Compat.Any<int>())
            .ReturnsForAnyArgs(Task.FromResult(true));

        recipientService.InsertManyRecipientReader(
                Arg.Compat.Any<IList<InsertOrRemoveManyRecipientModel>>(),
                Arg.Compat.Any<int>(),
                Arg.Compat.Any<int>())
            .ReturnsForAnyArgs(Task.FromResult(true));
        await preMatchNotificationService.NotifyToReloadTicketInPrematchTable(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<List<string>>());

        messageService.InsertMessage(request, Arg.Any<int>()).Returns(new NewMessageRequestModel());

        mapper
            .Map<NewMessageRequestModel>(request)
            .Returns(_ => new NewMessageRequestModel
            {
                Message = "abc"
            }
            );

        await notificationService.NotifyNewMessageIncome(Arg.Compat.Any<List<string>>());

        var actual = await handler.Handle(request, CancellationToken.None);

        // Temp
        //Assert
        const int expected = 111;
        Assert.Equal(expected, actual);
    }
}