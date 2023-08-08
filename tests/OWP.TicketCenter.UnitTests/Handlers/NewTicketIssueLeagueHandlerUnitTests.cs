using AutoMapper;
using Fanex.Data.Repository;
using NSubstitute;
using OWP.RabbitMQ;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Handlers;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Notification.Services;
using OWP.TicketCenter.Services;
using Xunit;

namespace OWP.TicketCenter.UnitTests.Handlers;

public class NewTicketIssueLeagueHandlerUnitTests
{
    private readonly Repository repository;

    private readonly IMapper mapper;


    private readonly NotificationService notificationService;

    private readonly RecipientService recipientService;

    private readonly NewTicketIssueLeagueHandler handler;

    private readonly MessageService messageService;

    private readonly PreMatchNotificationService preMatchNotificationService;

    public NewTicketIssueLeagueHandlerUnitTests()
    {
        repository = Substitute.For<Repository>();
        mapper = Substitute.For<IMapper>();
        messageService = Substitute.For<MessageService>();
        notificationService = Substitute.For<NotificationService>();
        recipientService = Substitute.For<RecipientService>();
        var messageClient = Substitute.For<MessageClient>();
        preMatchNotificationService = Substitute.For<PreMatchNotificationService>();

        handler = new NewTicketIssueLeagueHandler(
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
    public async Task InsertNewLeagueIssue_IssueInfo_ReturnTicketId()
    {
        // Arrange
        var request = new NewTicketIssueLeagueRequest();

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

        //Assert
        const int expected = 111;
        Assert.Equal(expected, actual);
    }
}