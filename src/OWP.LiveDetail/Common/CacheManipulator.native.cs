using Fanex.Caching;
using Newtonsoft.Json;
using OWP.LiveDetail.MatchInfo;

namespace OWP.LiveDetail.Common;

public interface LiveDetailCacheManipulator
{
    Task SaveOldAssignmentSnapshot(string sessionId, AssignmentSnapshot assignmentSnapshot);
}

public class LiveDetailCacheManipulatorImp : LiveDetailCacheManipulator
{
    private readonly ICache cache;
    private const string KEY_ASSIGNMENT_SNAPSHOT = "OWP-ASSIGNMENT-SNAPSHOT";
    private const int CACHE_EXPIRED_IN_30_MINS = 30;

    public LiveDetailCacheManipulatorImp(ICache cache)
    {
        this.cache = cache;
    }

    public async Task SaveOldAssignmentSnapshot(string sessionId, AssignmentSnapshot assignmentSnapshot)
    {
        var cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CACHE_EXPIRED_IN_30_MINS));
        var assignmentSnapshotInJson = JsonConvert.SerializeObject(assignmentSnapshot);
        var cacheKey = $"{KEY_ASSIGNMENT_SNAPSHOT}-{sessionId}";

        await cache.SetAsync(cacheKey, assignmentSnapshotInJson, cachePolicy);
    }
}