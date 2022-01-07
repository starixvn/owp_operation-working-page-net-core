using Fanex.NAP.Client.Helpers.Web;

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
        private readonly INapSession napSession;

        private readonly PreMatchAssignmentService service;

        public OnClickIssueNotificationHandler(INapSession napSession, PreMatchAssignmentService service)
        {
            this.napSession = napSession;
            this.service = service;
        }

        public async Task<Unit> Handle(OnClickIssueNotificationRequest request, CancellationToken cancellationToken)
        {
            var actorId = napSession.UserId;
            var issueInfo = await service.QueryPreMatchIssueInfo(request.PreMatchIssueId);

            //PreMatchIssueNotifyHub.OpenIssueChatBoard(actorId, issueInfo);

            return await Task.FromResult(Unit.Value);
        }
    }
}
