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

public class NewTicketIssueSupportHandlerUnitTests
{
    private readonly Repository repository;

    private readonly IMapper mapper;


    private readonly NotificationService notificationService;

    private readonly RecipientService recipientService;

    private readonly NewTicketIssueSupportHandler handler;

    private readonly MessageService messageService;

    public NewTicketIssueSupportHandlerUnitTests()
    {
        repository = Substitute.For<Repository>();
        mapper = Substitute.For<IMapper>();
        notificationService = Substitute.For<NotificationService>();
        recipientService = Substitute.For<RecipientService>();
        var messageClient = Substitute.For<MessageClient>();
        messageService = Substitute.For<MessageService>();

        handler = new NewTicketIssueSupportHandler(
            repository,
            messageService,
            notificationService,
            recipientService,
            messageClient,
            mapper
        );
    }

    [Fact]
    public async Task InsertNewSupportIssue_IssueInfo_ReturnTicketId()
    {
        // Arrange
        var request = new NewTicketIssueSupportRequest();

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