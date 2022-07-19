using Fanex.Caching;
using Fanex.Logging;
using StackExchange.Redis;
using Newtonsoft.Json;
using OWP.NotificationRequests;
using MediatR;

namespace OWP.RedisPubSub;
public interface PubSubEvent
{
    void Subscribe(string chanel);
}

public class PubSubEventImp : PubSubEvent
{
    private readonly IConnectionMultiplexer redis;

    private readonly IMediator mediator;

    public PubSubEventImp(IConnectionMultiplexer redis, IMediator mediator)
    {
        this.redis = redis;
        this.mediator = mediator;
    }

    public void Subscribe(string channel)
    {
        var sub = redis.GetSubscriber();

        sub.Subscribe(channel, (channel, message) =>
        {
            var request = JsonConvert.DeserializeObject<PrematchNewLeagueNotifyRequest>(message);
            mediator.Send(request).ConfigureAwait(false);
        });
    }
}