namespace OWP.PrematchAssignment.Handlers
{
    using MediatR;
    using Models;
    using PreMatchAssignment.Services;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class QueryAssigneesRequest : IRequest<IEnumerable<TicketAssignee>>
    {
        public int Id { get; set; }
    }

    public class QueryPreMatchAssigneesHandler : IRequestHandler<QueryAssigneesRequest, IEnumerable<TicketAssignee>>
    {
        private readonly PreMatchAssignmentService preMatchAssignmentService;

        public QueryPreMatchAssigneesHandler(PreMatchAssignmentService preMatchAssignmentService)
        {
            this.preMatchAssignmentService = preMatchAssignmentService;
        }

        public async Task<IEnumerable<TicketAssignee>> Handle(QueryAssigneesRequest request, CancellationToken cancellationToken)
        {
            var result = await preMatchAssignmentService.QueryAssigneesByLeagueId(request.Id);

            return result.ToList();
        }
    }
}

