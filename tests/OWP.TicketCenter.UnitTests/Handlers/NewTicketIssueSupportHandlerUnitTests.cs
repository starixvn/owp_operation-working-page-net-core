namespace OWP.TicketCenter.UnitTests.Handlers;

using AutoMapper;
using Criteria;
using Models;
using Notification.Services;
using NSubstitute;
using RabbitMQ;
using TicketCenter.Handlers;
using TicketCenter.Services;
using Xunit;

public class NewTicketIssueSupportHandlerUnitTests
{
    private readonly Repository repository;

    private readonly IMapper mapper;


    private readonly NotificationService notificationService;

    private readonly RecipientService recipientService;

    private readonly NewTicketIssueSupportHandler handler;

    private readonly MessageClient messageClient;

    private readonly MessageService messageService;

    public NewTicketIssueSupportHandlerUnitTests()
    {
        repository = Substitute.For<Repository>();
        mapper = Substitute.For<IMapper>();
        notificationService = Substitute.For<NotificationService>();
        recipientService = Substitute.For<RecipientService>();
        messageClient = Substitute.For<MessageClient>();
        messageService = Substitute.For<MessageService>();

        handler = new NewTicketIssueSupportHandler(
            repository,
            messageService,
            notificationService,
            recipientService,
            messageClient
        );
    }

    [Fact]
    public async void InsertNewSupportIssue_IssueInfo_ReturnTicketId()
    {
        // Arrange
        var request = new NewTicketIssueSupportRequest();

        repository
            .BuildTicketId(request.TicketType, request.SubType)
            .Returns(Task.FromResult(111));

        await repository
            .AddNewTicketIssueSupport(request);

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
            .Returns(x => new NewMessageRequestModel
                {
                    Message = "abc"
                }
            );


        await notificationService.NotifyNewMessageIncome(Arg.Compat.Any<int>());

        var actual = await handler.Handle(request, CancellationToken.None);

        //Assert
        var expected = 111;
        Assert.Equal(actual, expected);
    }
}