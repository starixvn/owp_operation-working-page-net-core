using AutoMapper;
using NSubstitute;
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

        public NewTicketIssueLeagueHandlerUnitTests()
        {
            repository = Substitute.For<Repository>();
            mapper = Substitute.For<IMapper>();
            notificationService = Substitute.For<NotificationService>();
            recipientService = Substitute.For<RecipientService>();

            //handler = new NewTicketIssueLeagueHandler(
            //    repository,
            //    mapper,
            //    writer,
            //    notificationService,
            //    recipientService
            //    );
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