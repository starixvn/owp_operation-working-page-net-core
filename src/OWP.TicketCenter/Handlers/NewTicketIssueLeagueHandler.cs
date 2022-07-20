using AutoMapper;
using MediatR;
using OWP.MessagePublisher;
using OWP.NotificationHandler;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Notification.Models;
using OWP.TicketCenter.Notification.Services;
using OWP.TicketCenter.Services;

namespace OWP.TicketCenter.Handlers
{
    public class NewTicketIssueLeagueRequest : NewTicketModelBase, IRequest<int>
    {
        public int OwpLeagueId { get; set; }
    }

    public class NewTicketIssueLeagueHandler : IRequestHandler<NewTicketIssueLeagueRequest, int>
    {
        private readonly Repository repository;
        private readonly IMapper mapper;
        private readonly NewTicketLeagueMessageDBWriter writer;
        private readonly NotificationService notificationService;
        private readonly RecipientService recipientService;
        private readonly IMessagePublisher publisher;
        private readonly MessageClient messageClient;

        public NewTicketIssueLeagueHandler(
            Repository repository,
            IMapper mapper,
            NewTicketLeagueMessageDBWriter writer,
            NotificationService notificationService,
            RecipientService recipientService,
            IMessagePublisher publisher,
            MessageClient messageClient)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.writer = writer;
            this.notificationService = notificationService;
            this.recipientService = recipientService;
            this.publisher = publisher;
            this.messageClient = messageClient;
        }

        public async Task<int> Handle(NewTicketIssueLeagueRequest request, CancellationToken cancellationToken)
        {
            var ticketId = await repository.BuildTicketId(request.TicketType, request.SubType);
            request.TicketId = ticketId;
            await repository.AddNewTicketIssueLeague(request);

            var criteria = new InsertIssueCreatorCriteria
            {
                TicketId = ticketId,
                CreatorId = request.CreatorId
            };

            await repository.InsertIssueCreator(criteria);

            var messages = new List<NewMessageRequestModel>();
            foreach (var file in request.Files)
            {
                messages.Add(new()
                {
                    TicketId = ticketId,
                    CreatorId = request.CreatorId,
                    Message = file.Title,
                    FileId = file.FileId
                });
            }
            var newMessage = mapper.Map<NewMessageRequestModel>(request);
            messages.Add(newMessage);

            foreach (var message in messages)
            {
                await repository.AddNewMessage(message);
            }

            await recipientService.InsertManyRecipient(request.Recipients, request.TicketId);

            await recipientService.InsertManyRecipientReader(request.Recipients, request.TicketId, request.CreatorId);

            var notificationMessage = new MessageTransform
            {
                TicketId = ticketId,
                Comment = newMessage.Message,
                ActorId = request.CreatorId
            };

            //
            messageClient.SendMessage(request, RoutingKey.TicketCenterNewTicketLeague);
            //var sessionId = await writer.Execute(
            //    notificationMessage,
            //    NotificationTemplateCode.NewTicket);

            //await notificationService.Notify(new NotificationSource
            //{
            //    SessionId = sessionId,
            //    TicketId = ticketId,
            //    EventType = (int)EventType.League,
            //});

            await notificationService.NotifyNewMessageIncome(ticketId);

            await notificationService.NotifyNewAddRecipient(ticketId, request.CreatorId);



            return ticketId;
        }
    }
}