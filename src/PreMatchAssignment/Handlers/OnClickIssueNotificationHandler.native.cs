namespace OWP.PreMatchAssignment.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Fanex.NAP.Core.Web.Session;
    using MediatR;
    using Models;
    using Services;

    public class OnClickIssueNotificationRequest : IRequest
    {
        public int PreMatchIssueId { get; set; }
    }

    public class OnClickIssueNotificationHandler : IRequestHandler<OnClickIssueNotificationRequest>
    {
        private readonly ICurrentSession currentSession;

        private readonly PreMatchAssignmentService service;

        public OnClickIssueNotificationHandler(ICurrentSession currentSession, PreMatchAssignmentService service)
        {
            this.currentSession = currentSession;
            this.service = service;
        }

        public async Task<Unit> Handle(OnClickIssueNotificationRequest request, CancellationToken cancellationToken)
        {
            var actorId = currentSession.Username;
            var issueInfo = await service.QueryPreMatchIssueInfo(request.PreMatchIssueId);

            //PreMatchIssueNotifyHub.OpenIssueChatBoard(actorId, issueInfo);

            return await Task.FromResult(Unit.Value);
        }
    }
}
