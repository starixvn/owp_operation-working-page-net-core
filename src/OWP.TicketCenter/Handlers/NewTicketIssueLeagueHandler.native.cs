using System.Collections.Generic;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.Handlers
{
    public class NewTicketIssueLeagueRequest : NewTicketModelBase, IRequest<int>
    {
        public int OwpLeagueId { get; set; }
       
    }

    public class NewTicketIssueLeagueHandler : IRequestHandler<NewTicketIssueLeagueRequest, int>
    {
        private readonly Repository repository;
        public NewTicketIssueLeagueHandler(Repository repository)
        {
            this.repository = repository;
        }
        public async Task<int> Handle(NewTicketIssueLeagueRequest request, CancellationToken cancellationToken)
        {
            var ticketId = await repository.BuildTicketId(request.TicketType);
            request.TicketId = ticketId;
            await repository.AddNewTicketIssueLeague(request);
            await repository.AddNewMessage(ticketId, request.CreatorId, request.Message);
           
            return ticketId;
        }
    }
}
