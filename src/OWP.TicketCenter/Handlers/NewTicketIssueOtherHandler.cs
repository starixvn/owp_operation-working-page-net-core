using System.Collections.Generic;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using OWP.TicketCenter.Enum;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Notification.Const;
using OWP.TicketCenter.Notification.Models;
using OWP.TicketCenter.Notification.Services;

namespace OWP.TicketCenter.Handlers
{

    public class NewTicketIssueOtherRequest : NewTicketModelBase, IRequest<int>
    {
        public string Subject { get; set; }
    }

    public class NewTicketIssueOtherHandler : IRequestHandler<NewTicketIssueOtherRequest, int>
    {
        private readonly Repository repository;

        private readonly IMapper mapper;

        private readonly NewTicketOtherMessageDBWriter writer;


        private readonly NotificationService notificationService;

        private readonly IMediator mediator;

        public NewTicketIssueOtherHandler(
            Repository repository,
            IMapper mapper,
            NewTicketOtherMessageDBWriter writer,
            NotificationService notificationService,
            IMediator mediator)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.writer = writer;
            this.notificationService = notificationService;
            this.mediator = mediator;
        }

        public async Task<int> Handle(NewTicketIssueOtherRequest request, CancellationToken cancellationToken)
        {
            var ticketId = await repository.BuildTicketId(request.TicketType);
            request.TicketId = ticketId;
            await repository.AddNewTicketIssueOther(request);
            await repository.InsertIssueCreator(ticketId, request.CreatorId);

            var messages = new List<NewMessageRequestModel>();
            foreach (var file in request.Files)
            {
                messages.Add(new ()
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

            await mediator.Send(new NewTicketRecipientRequest
            {
                TicketId = ticketId,
                Recipients = request.Recipients
            }, cancellationToken);

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

            await notificationService.Notify(sessionId, ticketId, EventType.Other);

            return ticketId;
        }
    }
}
