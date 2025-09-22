using Fanex.Caching;
using Fanex.NAP.Client;
using MediatR;
using Newtonsoft.Json;
using OWP.CQRS;
using OWP.Extensions;
using OWP.PrematchAssignment.Constants;
using OWP.PrematchAssignment.QueryAndSave.Models;
using OWP.PrematchAssignment.QueryAndSave.Services;

namespace OWP.PrematchAssignment.QueryAndSave.Handlers;

public record CacheLeagueNameRequest(IList<string> LeagueName, DateTime RequestDate, bool isDay) : Command<Unit>;


public class CacheLeagueNameHandler(ICache cache, INapSession session, Repository repository) : CommandHandler<CacheLeagueNameRequest, Unit>
{

    public async Task<Unit> Handle(CacheLeagueNameRequest request, CancellationToken cancellationToken)
    {
        if (!request.LeagueName.Any())
        {
            return Unit.Value;
        }

        var key = Olympus.OWP.Cache.Utils.BuildPrematchNotificationCacheKey(request.RequestDate, request.isDay);
        var currentCacheInfo=await GetCacheInfo(key);
        var newLeagues = MergeLeagues(currentCacheInfo.Leagues, request.LeagueName);
        var newCache = JsonConvert.SerializeObject(new CacheInfo
        {
            Actor = session.UserName,
            Leagues = newLeagues
        });
        await Cache(key, newCache);
        await repository.LogDetailedPayload(DateTimeOffset.Now.ToGmt8().DateTime, "[Prematch_Notification]", newCache);
        return Unit.Value;
    }

    private async Task<CacheInfo> GetCacheInfo(string key)
    {
        var cacheResult = await cache.TryGetAsync<string>(key);
        return cacheResult.found
            ? JsonConvert.DeserializeObject<CacheInfo>(cacheResult.value)
            : new CacheInfo();
    }

    private static IList<string> MergeLeagues(
        IList<string> source, IList<string> target)
    =>  source.Union(target).ToList();

    private async Task Cache(string key, string cacheInfo)
    {
        var cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(CacheConst.CACHE_EXPIRED_IN_3_DAYS)); ;
        await cache.SetAsync(key, cacheInfo, cachePolicy);
    }

}