namespace OWP.TicketCenter.Handlers;

using MediatR;
using Middleware.Models;
using Notification.Services;

public class OnClickIssueNotificationRequest : PermissionCheckedModel, IRequest
{
    public int EventType { get; set; }
}

public class OnClickIssueNotificationRequestImp : IRequestHandler<OnClickIssueNotificationRequest>
{
    private readonly NotificationService notify;

    public OnClickIssueNotificationRequestImp(NotificationService notify)
    {
        this.notify = notify;
    }

    public async Task<Unit> Handle(OnClickIssueNotificationRequest request, CancellationToken cancellationToken)
    {
        await notify.NotifyUserClickOnToast(request.ActorId, request.TicketId, request.EventType);

        return await Task.FromResult(Unit.Value);
    }
}