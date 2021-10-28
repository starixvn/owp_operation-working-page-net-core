using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using NSubstitute;
using OWP.TicketCenter.Enum;
using OWP.TicketCenter.Handlers;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Notification.Const;
using OWP.TicketCenter.Notification.Models;
using OWP.TicketCenter.Notification.Services;
using OWP.TicketCenter.Services;
using Xunit;

namespace OWP.TicketCenter.UnitTests.Handlers
{
    public class NewTicketIssueOtherHandlerUnitTests
    {
        private readonly Repository repository;

        private readonly IMapper mapper;

        private readonly NewTicketOtherMessageDBWriter writer;

        private readonly NotificationService notificationService;

        private readonly RecipientService recipientService;

        private readonly NewTicketIssueOtherHandler handler;

        public NewTicketIssueOtherHandlerUnitTests()
        {
            repository = Substitute.For<Repository>();
            mapper = Substitute.For<IMapper>();
            writer = Substitute.For<NewTicketOtherMessageDBWriter>();
            notificationService = Substitute.For<NotificationService>();
            recipientService = Substitute.For<RecipientService>();

            handler = new NewTicketIssueOtherHandler(
                repository,
                mapper,
                writer,
                notificationService,
                recipientService
                );
        }

        [Fact]
        public async void InsertNewOtherIssue_IssueInfo_ReturnTicketId()
        {
            // Arrange
            var request = new NewTicketIssueOtherRequest();

            repository
                .BuildTicketId(request.TicketType)
                .Returns(Task.FromResult(111));

            await repository
                .AddNewTicketIssueOther(request);

            await repository.AddNewMessage(Arg.Compat.Any<NewMessageRequestModel>());

            await repository.InsertIssueCreator(Arg.Compat.Any<int>(), Arg.Compat.Any<int>());

            recipientService.InsertManyRecipient(
                    Arg.Compat.Any<IList<InsertOrRemoveManyRecipientModel>>(),
                    Arg.Compat.Any<int>())
                .ReturnsForAnyArgs(Task.FromResult(true));

            recipientService.InsertManyRecipientReader(
                    Arg.Compat.Any<IList<InsertOrRemoveManyRecipientModel>>(),
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
                Arg.Compat.Any<string>(),
                Arg.Compat.Any<int>(),
                Arg.Compat.Any<int>());

            await notificationService.NotifyNewMessageIncome(
                Arg.Compat.Any<int>(),
                Arg.Compat.Any<int>());

            var actual = await handler.Handle(request, CancellationToken.None);

            //Assert
            var expected = 111;
            Assert.Equal(actual, expected);
        }
    }
}