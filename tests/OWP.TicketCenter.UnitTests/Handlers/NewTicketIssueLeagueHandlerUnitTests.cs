using AutoMapper;
using NSubstitute;
using OWP.RabbitMQ;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Handlers;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Notification.Services;
using OWP.TicketCenter.Services;
using Xunit;

namespace OWP.TicketCenter.UnitTests.Handlers
{
    public class NewTicketIssueLeagueHandlerUnitTests
    {
        private readonly Repository repository;

        private readonly IMapper mapper;


        private readonly NotificationService notificationService;

        private readonly RecipientService recipientService;

        private readonly NewTicketIssueLeagueHandler handler;

        private readonly MessageClient messageClient;

        private readonly MessageService messageService;

        public NewTicketIssueLeagueHandlerUnitTests()
        {
            repository = Substitute.For<Repository>();
            mapper = Substitute.For<IMapper>();
            messageService = Substitute.For<MessageService>();
            notificationService = Substitute.For<NotificationService>();
            recipientService = Substitute.For<RecipientService>();
            messageClient = Substitute.For<MessageClient>();

            handler = new NewTicketIssueLeagueHandler(
                repository,
                messageService,
                notificationService,
                recipientService,
                messageClient
                );
        }

        [Fact]
        public async void InsertNewLeagueIssue_IssueInfo_ReturnTicketId()
        {
            // Arrange
            var request = new NewTicketIssueLeagueRequest();

            repository
                .BuildTicketId(request.TicketType, request.SubType)
                .Returns(Task.FromResult(111));

            await repository
                .AddNewTicketIssueLeague(request);

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
}