namespace OWP.PrematchAssignment.Handlers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Models;
    using PreMatchAssignment.Services;

    public class QueryAssigneeRequest : IRequest<IEnumerable<AssigneeQueried>>
    {
        public int Id { get; set; }
    }

    public class QueryAssigneePreMatchHandler : IRequestHandler<QueryAssigneeRequest, IEnumerable<AssigneeQueried>>
    {
        private readonly PreMatchAssignmentService preMatchAssignmentService;

        public QueryAssigneePreMatchHandler(PreMatchAssignmentService preMatchAssignmentService)
        {
            this.preMatchAssignmentService = preMatchAssignmentService;
        }

        public async Task<IEnumerable<AssigneeQueried>> Handle(QueryAssigneeRequest request, CancellationToken cancellationToken)
        {
            var result = await preMatchAssignmentService.QueryAssigneePreMatch(request.Id);

            return result.ToList();
        }
    }
}

