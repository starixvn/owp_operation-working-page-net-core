using AutoMapper;
using NSubstitute;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Handlers;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Notification.Const;
using OWP.TicketCenter.Notification.Models;
using OWP.TicketCenter.Notification.Services;
using OWP.TicketCenter.Services;

using Xunit;

namespace OWP.TicketCenter.UnitTests.Handlers
{
    public class NewTicketIssueEventHandlerUnitTests
    {
        private readonly Repository repository;

        private readonly IMapper mapper;

        private readonly NewTicketMatchMessageDBWriter writer;

        private readonly NotificationService notificationService;

        private readonly RecipientService recipientService;

        private readonly NewTicketIssueEventHandler handler;

        public NewTicketIssueEventHandlerUnitTests()
        {
            repository = Substitute.For<Repository>();
            mapper = Substitute.For<IMapper>();
            writer = Substitute.For<NewTicketMatchMessageDBWriter>();
            notificationService = Substitute.For<NotificationService>();
            recipientService = Substitute.For<RecipientService>();

            handler = new NewTicketIssueEventHandler(
                repository,
                mapper,
                writer,
                notificationService,
                recipientService
                );
        }

        [Fact]
        public async void InsertNewEventIssue_IssueInfo_ReturnTicketId()
        {
            // Arrange
            var request = new NewTicketIssueEventRequest();

            repository
                .BuildTicketId(request.TicketType)
                .Returns(Task.FromResult(111));

            await repository
                .AddNewTicketIssueEvent(request);

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

            await writer.Execute(Arg.Compat.Any<MessageTransform>(), Arg.Compat.Any<NotificationTemplateCode>());

            await notificationService.Notify(
                Arg.Compat.Any<NotificationSource>());

            await notificationService.NotifyNewMessageIncome(Arg.Compat.Any<int>());

            var actual = await handler.Handle(request, CancellationToken.None);

            //Assert
            var expected = 111;
            Assert.Equal(actual, expected);
        }
    }
}