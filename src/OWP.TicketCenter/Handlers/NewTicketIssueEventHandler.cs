using AutoMapper;
using MediatR;
using OWP.TicketCenter.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OWP.TicketCenter.Notification.Const;
using OWP.TicketCenter.Notification.Models;
using OWP.TicketCenter.Notification.Services;

namespace OWP.TicketCenter.Handlers
{
    public class NewTicketIssueEventRequest : NewTicketModelBase, IRequest<int>
    {
        public int InfoSiteMatchId { get; set; }

        public int OwpLeagueId { get; set; }

        public int? OwpHomeId { get; set; }

        public int? OwpAwayId { get; set; }
    }

    public class NewTicketIssueEventHandler : IRequestHandler<NewTicketIssueEventRequest, int>
    {
        private readonly Repository repository;

        private readonly IMapper mapper;

        private readonly NewTicketMatchMessageDBWriter writer;


        public NewTicketIssueEventHandler(Repository repository, IMapper mapper, NewTicketMatchMessageDBWriter writer)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.writer = writer;
        }

        public async Task<int> Handle(NewTicketIssueEventRequest request, CancellationToken cancellationToken)
        {
            var ticketId = await repository.BuildTicketId(request.TicketType);
            request.TicketId = ticketId;
            await repository.AddNewTicketIssueEvent(request);

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
            await repository.InsertIssueCreator(ticketId, request.CreatorId);
            var notificationMessage = new Message
            {
                TicketId = ticketId,
                Comment = newMessage.Message,
                ActorId = request.CreatorId
            };
            var sessionId = await writer.Execute(
                notificationMessage,
                NotificationTemplateCode.NewTicket);
            return ticketId;
        }
    }
}