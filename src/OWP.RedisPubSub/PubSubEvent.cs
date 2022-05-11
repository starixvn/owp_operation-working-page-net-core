using Fanex.Caching;
using Fanex.Logging;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace OWP.RedisPubSub;
public interface PubSubEvent
{
  void Publish();

  void Subscribe();
}
public class PubSubEventImp : PubSubEvent
{
  private readonly ICache cache;

  private readonly ILogger logger;

  private readonly IConnectionMultiplexer redis;

  public PubSubEventImp(ICache cache, ILogger logger, IConnectionMultiplexer redis)
  {
    this.cache = cache;
    this.logger = logger;
    this.redis= redis;
  }

  public void Publish()
  {
    
  }

  public void Subscribe()
  {
    logger.Info("Redis Subscribe start");
   
     logger.Info("Redis Connect");
      var cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30));
    cache.Set($"RedisPubSub-Subscribe-{DateTime.Now}","RedisPubSub-Subscribe" , cachePolicy);
   
    var sub = redis.GetSubscriber();
    sub.Subscribe("messages", (channel, message) =>
    {
       var policy = new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30));
      var body=JsonConvert.SerializeObject(message);
      logger.Info($"Redis Subscribe receive: {body}");
      cache.Set($"RedisPubSub-Subscribe-{body}", message, policy);
    });
  }
}