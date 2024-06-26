
using OWP.Services;

namespace OWP.BackgroundWorker.Handler;

public class NotificationQueueRequest: Query<Unit>
{
}

public class NotificationQueueHandler : QueryHandler<NotificationQueueRequest, Unit>
{
    private readonly IDynamicRepository repository;
    private readonly OldNotificationService notify;

    public NotificationQueueHandler(IDynamicRepository repository, OldNotificationService notify)
    {
        this.repository = repository;
        this.notify = notify;
    }

    public async Task<Unit> Handle(NotificationQueueRequest request, CancellationToken cancellationToken)
    {
        var sessionIds = await repository.FetchAsync<string>(new GetNotificationQueueCriteria());
        foreach (var sessionId in sessionIds)
        {
            await notify.Notify(sessionId);
        }
        return Unit.Value;
    }
}
