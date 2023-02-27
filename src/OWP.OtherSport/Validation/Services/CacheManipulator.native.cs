using Fanex.Caching;
using Newtonsoft.Json;
using OWP.OtherSport.Validation.Models;

namespace OWP.OtherSport.Validation.Services;

public interface CacheManipulator
{
    Task SaveAssignmentsSnapshot(string sessionId, List<AssignmentModel> assignments);
}

public class CacheManipulatorImp : CacheManipulator
{
    private readonly ICache cache;

    private const string KEY_OS_ASSIGNMENT = "OWP-OS-AssignmentUpload";
    private const int ASSIGNMENT_CACHE_EXPIRED_IN_MINUTE = 15;

    public CacheManipulatorImp(
        ICache cache)
    {
        this.cache = cache;
    }

    public async Task SaveAssignmentsSnapshot(string sessionId, List<AssignmentModel> assignments)
    {
        var absoluteTimeSpan = TimeSpan.FromMinutes(ASSIGNMENT_CACHE_EXPIRED_IN_MINUTE);
        var cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(absoluteTimeSpan);
        var cacheKey = $"{KEY_OS_ASSIGNMENT}-{sessionId}";

        var assignmentsJson = JsonConvert.SerializeObject(assignments);
        await cache.SetAsync(cacheKey, assignmentsJson, cachePolicy);
    }
}