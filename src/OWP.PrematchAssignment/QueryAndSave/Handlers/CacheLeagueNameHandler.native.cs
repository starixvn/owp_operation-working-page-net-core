using Fanex.Caching;
using MediatR;
using Newtonsoft.Json;
using OWP.CQRS;
using OWP.PrematchAssignment.Constants;

namespace OWP.PrematchAssignment.QueryAndSave.Handlers;

public record CacheLeagueNameRequest(IList<string> LeagueName, DateTime RequestDate, bool isDay) : Command<Unit>;


public class CacheLeagueNameHandler(ICache cache) : CommandHandler<CacheLeagueNameRequest, Unit>
{

    public async Task<Unit> Handle(CacheLeagueNameRequest request, CancellationToken cancellationToken)
    {
        if (!request.LeagueName.Any())
        {
            return Unit.Value;
        }

        //var key = BuildKey(leagueNameRequest);
        var key = Olympus.OWP.Cache.Utils.BuildPrematchNotificationCacheKey(request.RequestDate, request.isDay);
        var leaguesFromCache=await GetLeaguesFromCache(key);
        var leagues = MergeLeagues(leaguesFromCache, request.LeagueName);
        await Cache(key, leagues);
        return Unit.Value;
    }

    private async Task<IList<string>> GetLeaguesFromCache(string key)
    {
        var cacheResult = await cache.TryGetAsync<IList<string>>(key);
        return cacheResult.Item2 ?? [];
    }

    private static IList<string> MergeLeagues(
        IList<string> source, IList<string> target)
    =>  source.Union(target).ToList();

    private async Task Cache(string key, IList<string> leagues)
    {
        var cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(CacheConst.CACHE_EXPIRED_IN_3_DAYS));
        var assignmentSnapshotInJson = JsonConvert.SerializeObject(leagues);
        await cache.SetAsync(key, assignmentSnapshotInJson, cachePolicy);
    }

}