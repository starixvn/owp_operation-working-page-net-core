using Fanex.Caching;
using MediatR;
using Newtonsoft.Json;
using OWP.CQRS;
using OWP.PrematchAssignment.Constants;

namespace OWP.PrematchAssignment.QueryAndSave.Handlers;

public record CacheLeagueNameRequest(IList<string> LeagueName, DateTime RequestDate, bool isDay) : Command<Unit>;


public class CacheLeagueNameHandler(ICache cache) : CommandHandler<CacheLeagueNameRequest, Unit>
{

    public async Task<Unit> Handle(CacheLeagueNameRequest leagueNameRequest, CancellationToken cancellationToken)
    {
        if (!leagueNameRequest.LeagueName.Any())
        {
            return Unit.Value;
        }

        var key = BuildKey(leagueNameRequest);
        var leaguesFromCache=await GetLeaguesFromCache(key);
        var leagues = MergeLeagues(leaguesFromCache, leagueNameRequest.LeagueName);

        await RemoveExistsCache(key);
        await Cache(key, leagues);
        return Unit.Value;
    }

    private string BuildKey(CacheLeagueNameRequest request)
        => request.isDay
            ? $"{CacheConst.PREMATCH_SHIFT}-{request.RequestDate:yyyy-MM-dd}-day-shift"
            : $"{CacheConst.PREMATCH_SHIFT}-{request.RequestDate:yyyy-MM-dd}-night-shift";

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
        var cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CacheConst.CACHE_EXPIRED_IN_3_DAYS));
        var assignmentSnapshotInJson = JsonConvert.SerializeObject(leagues);
        await cache.SetAsync(key, assignmentSnapshotInJson, cachePolicy);
    }

    private async Task RemoveExistsCache(string key)
    {
        if (await cache.ContainAsync(key))
        {
            await cache.RemoveAsync(key);
        }
    }
}