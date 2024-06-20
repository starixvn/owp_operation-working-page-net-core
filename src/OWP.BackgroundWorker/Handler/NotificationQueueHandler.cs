
namespace OWP.BackgroundWorker.Handler;

public class NotificationQueueRequest: Query<Unit>
{
}

public class NotificationQueueHandler : QueryHandler<NotificationQueueRequest, Unit>
{
    private readonly IDynamicRepository repository;

    public async Task<Unit> Handle(NotificationQueueRequest request, CancellationToken cancellationToken)
    {
        var sessionIds = await repository.FetchAsync<string>(new GetNotificationQueueCriteria(), cancellationToken);
        foreach (var sessionId in sessionIds)
        {
           // TODO
        }
        return Unit.Value;
    }
}
