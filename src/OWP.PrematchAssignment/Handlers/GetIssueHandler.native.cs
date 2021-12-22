namespace OWP.PreMatchAssignment.Handlers
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Models;
    using Services;

    public class GetIssueRequest : IRequest<IEnumerable<Message>>
    {
        public int PrematchIssueId { get; set; }

        public bool IsShowAllMessage { get; set; }
    }
    public class GetIssueHandler : IRequestHandler<GetIssueRequest, IEnumerable<Message>>
    {
        private readonly PreMatchAssignmentService preMatchAssignmentService;

        public GetIssueHandler(PreMatchAssignmentService preMatchAssignmentService)
        {
            this.preMatchAssignmentService = preMatchAssignmentService;
        }

        public async Task<IEnumerable<Message>> Handle(GetIssueRequest request, CancellationToken cancellationToken)
        {
            var result = await preMatchAssignmentService.QueryIssueMessages(request.PrematchIssueId, request.IsShowAllMessage);

            return result;
        }
    }
}
