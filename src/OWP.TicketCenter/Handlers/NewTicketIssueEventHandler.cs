using AutoMapper;
using MediatR;
using OWP.TicketCenter.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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

        public NewTicketIssueEventHandler(Repository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
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
            return ticketId;
        }
    }
}