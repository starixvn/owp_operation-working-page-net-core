
using OWP.Services;

namespace OWP.BackgroundWorker.Handler;

public class NotificationQueueRequest: Query<Unit>
{
}

public class NotificationQueueHandler(IDynamicRepository repository, OldNotificationService notify) : QueryHandler<NotificationQueueRequest, Unit>
{
    public async Task<Unit> Handle(NotificationQueueRequest request, CancellationToken cancellationToken)
    {
        var sessionIds = await repository.FetchAsync<string>(new GetNotificationQueueCriteria(), cancellationToken);
        foreach (var sessionId in sessionIds)
        {
            await notify.Notify(sessionId);
        }
        return Unit.Value;
    }
}
