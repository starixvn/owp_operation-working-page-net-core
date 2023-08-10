using MediatR;
using OWP.TicketCenter.Middleware.Models;
using OWP.TicketCenter.Notification.Services;

namespace OWP.TicketCenter.Handlers;

public class OnClickIssueNotificationRequest : PermissionCheckedModel, IRequest<Unit>
{
    public int EventType { get; set; }
}

public class OnClickIssueNotificationRequestImp : IRequestHandler<OnClickIssueNotificationRequest, Unit>
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