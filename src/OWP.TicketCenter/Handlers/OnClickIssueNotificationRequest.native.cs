using System.Threading;
using System.Threading.Tasks;
using Fanex.NAP.Client.Helpers.Web;
using MediatR;
using OWP.TicketCenter.Notification.Services;

namespace OWP.TicketCenter.Handlers
{
    public class OnClickIssueNotificationRequest: IRequest
    {
        public int TicketId { get; set; }

        public int EventType { get; set; }
    }

    public class OnClickIssueNotificationRequestImp: IRequestHandler<OnClickIssueNotificationRequest>
    {
        private readonly INapSession currentSession;

        private readonly NotificationService notify;

        public OnClickIssueNotificationRequestImp(INapSession currentSession, NotificationService notify)
        {
            this.currentSession = currentSession;
            this.notify = notify;
        }

        public async Task<Unit> Handle(OnClickIssueNotificationRequest request, CancellationToken cancellationToken)
        {
            var actorId = currentSession.UserId;

            await notify.NotifyUserClickOnToast(actorId, request.TicketId, request.EventType);

            return await Task.FromResult(Unit.Value);
        }
    }
}
