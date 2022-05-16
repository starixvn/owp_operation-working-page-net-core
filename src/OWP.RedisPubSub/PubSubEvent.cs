using Fanex.Caching;
using Fanex.Logging;
using StackExchange.Redis;
using Newtonsoft.Json;
using OWP.NotificationRequests;
using OWP.RedisPubSub.Constants;
using MediatR;

namespace OWP.RedisPubSub;
public interface PubSubEvent
{
  void Subscribe();
}

public class PubSubEventImp : PubSubEvent
{
  private readonly ICache cache;

  private readonly ILogger logger;

  private readonly IConnectionMultiplexer redis;

  private readonly IMediator mediator;

  public PubSubEventImp(ICache cache, ILogger logger, IConnectionMultiplexer redis, IMediator mediator)
  {
    this.cache = cache;
    this.logger = logger;
    this.redis= redis;
    this.mediator = mediator;
  }

  public void Subscribe()
  {
    logger.Info("Redis Subscribe Connect");
    var sub = redis.GetSubscriber();
    
    sub.Subscribe(Constant.NEW_LEAGUE_CHANEL, (channel, message) =>
    {
       var request=JsonConvert.DeserializeObject<PrematchNewLeagueNotifyRequest>(message);
      logger.Info($"Redis Subscribe receive: {request?.SessionId}-{request?.FromDate}-{DateTime.Now}");
      mediator.Send(request).ConfigureAwait(false);
    });
  }
}