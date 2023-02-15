namespace OWP.LiveDetail.Common;

using Fanex.Caching;
using MatchInfo;
using Newtonsoft.Json;
using Services;

public interface LiveDetailCacheManipulator
{
    Task SaveOldAssignmentSnapshot(string sessionId, AssignmentSnapshot assignmentSnapshot);
}

public class LiveDetailCacheManipulatorImp : LiveDetailCacheManipulator
{
    private readonly ICache cache;
    private const string KEY_ASSIGNMENT_SNAPSHOT = "OWP-ASSIGNMENT-SNAPSHOT";
    private const int CACHE_EXPIRED_IN_DAYS = 1;
    private readonly LogServiceWrapper logService;

    public LiveDetailCacheManipulatorImp(ICache cache,
        LogServiceWrapper logService)
    {
        this.cache = cache;
        this.logService = logService;
    }

    public async Task SaveOldAssignmentSnapshot(string sessionId, AssignmentSnapshot assignmentSnapshot)
    {
        var cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(CACHE_EXPIRED_IN_DAYS));
        var assignmentSnapshotInJson = JsonConvert.SerializeObject(assignmentSnapshot);
        var cacheKey = $"{KEY_ASSIGNMENT_SNAPSHOT}-{sessionId}";

        await logService.Log($"Linked-match-KEY-NETFW-{sessionId}", cacheKey);

        await cache.SetAsync(cacheKey, assignmentSnapshotInJson, cachePolicy);
    }
}