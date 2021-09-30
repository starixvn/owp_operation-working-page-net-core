using System.Collections.Generic;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.Handlers
{
    public class NewTicketIssueEventRequest : NewTicketModelBase, IRequest<int>
    {
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
            var newMessage = mapper.Map<NewMessageRequestModel>(request);
            await repository.AddNewMessage(newMessage);
            await repository.AddFileInfo(newMessage);
            return ticketId;
        }
    }
}
