using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using OWP.TicketCenter.Enum;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Notification.Const;
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

        public NewTicketIssueLeagueHandler(
            Repository repository,
            IMapper mapper,
            NewTicketLeagueMessageDBWriter writer,
            NotificationService notificationService,
            RecipientService recipientService)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.writer = writer;
            this.notificationService = notificationService;
            this.recipientService = recipientService;
        }

        public async Task<int> Handle(NewTicketIssueLeagueRequest request, CancellationToken cancellationToken)
        {
            var ticketId = await repository.BuildTicketId(request.TicketType);
            request.TicketId = ticketId;
            await repository.AddNewTicketIssueLeague(request);
            await repository.InsertIssueCreator(ticketId, request.CreatorId);

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

            await recipientService.InsertManyRecipientReader(request.Recipients, request.TicketId);

            var notificationMessage = new MessageTransform
            {
                TicketId = ticketId,
                Comment = newMessage.Message,
                ActorId = request.CreatorId
            };
            var sessionId = await writer.Execute(
                notificationMessage,
                NotificationTemplateCode.NewTicket);

            await notificationService.NotifyNewMessageIncome(ticketId, request.CreatorId);

            await notificationService.NotifyNewAddRecipient(ticketId, request.CreatorId);

            await notificationService.Notify(sessionId, ticketId, (int)EventType.League);
            return ticketId;
        }
    }
}