namespace OWP.AssignmentUpload.Submit.Services;

using Fanex.Caching;
using Models;
using Newtonsoft.Json;
using Validation.Models;

public interface CacheManipulator
{
    Task DeleteAssignments(string sessionId);

    Task<List<AssignmentModel>> GetAssignments(string sessionId);

    Task SaveAssignmentSnapshot(string sessionId, AssignmentSnapshot assignmentSnapshot);

    Task<string> GetUserWhoIsUploadingAssignment(DateTime eventDate);

    Task SaveUserWhoIsUploadingAssignment(DateTime eventDate, string username);

    Task DeleteUserWhoIsUploadingAssignment(DateTime eventDate);

    Task SaveDeletedInfoSiteMatches(IEnumerable<MatchKickoffDate> matches);
}

public class CacheManipulatorImp : CacheManipulator
{
    private readonly ICache cache;

    private const string KEY_ASSIGNMENT = "OWP-AssignmentUpload";
    private const string KEY_USER_WHO_IS_ULOADING_ASSIGNMENT = "OWP-User-Who-Is-Uploading-Assignemnt";
    private const string KEY_ASSIGNMENT_SNAPSHOT = "OWP-ASSIGNMENT-SNAPSHOT";
    private const string KEY_DELETED_INFO_SITE_MATCH_ID = "OWP-DELETED-INFO-SITE-MATCH-ID";
    private const int CACHE_EXPIRED_IN_5_MINS = 5;
    private const int CACHE_USER_WHO_IS_ULOADING_ASSIGNMENT_EXPIRED_IN_MINITE = 30;

    public CacheManipulatorImp(ICache cache)
    {
        this.cache = cache;
    }

    public async Task SaveAssignmentSnapshot(string sessionId, AssignmentSnapshot assignmentSnapshot)
    {
        var cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CACHE_EXPIRED_IN_5_MINS));
        var assignmentSnapshotInJson = JsonConvert.SerializeObject(assignmentSnapshot);
        var cacheKey = $"{KEY_ASSIGNMENT_SNAPSHOT}-{sessionId}";
        await cache.SetAsync(cacheKey, assignmentSnapshotInJson, cachePolicy);
    }

    public async Task<List<AssignmentModel>> GetAssignments(string sessionId)
    {
        var cacheKey = $"{KEY_ASSIGNMENT}-{sessionId}";
        var cacheResult = await cache.TryGetAsync<string>(cacheKey);

        var assignments = string.IsNullOrEmpty(cacheResult.Item2)
            ? new List<AssignmentModel>()
            : JsonConvert.DeserializeObject<List<AssignmentModel>>(cacheResult.Item2);

        return assignments ?? new List<AssignmentModel>();
    }

    public async Task DeleteAssignments(string sessionId)
    {
        var cacheKey = $"{KEY_ASSIGNMENT}-{sessionId}";
        await cache.RemoveAsync(cacheKey);
    }

    public async Task<string> GetUserWhoIsUploadingAssignment(DateTime eventDate)
    {
        var cacheKey = $"{KEY_USER_WHO_IS_ULOADING_ASSIGNMENT}-{eventDate}";
        var cacheResult = await cache.TryGetAsync<string>(cacheKey);

        return cacheResult.Item2 ?? string.Empty;
    }

    public async Task SaveUserWhoIsUploadingAssignment(DateTime eventDate, string username)
    {
        var cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CACHE_USER_WHO_IS_ULOADING_ASSIGNMENT_EXPIRED_IN_MINITE));
        var cacheKey = $"{KEY_USER_WHO_IS_ULOADING_ASSIGNMENT}-{eventDate}";
        await cache.SetAsync(cacheKey, username, cachePolicy);
    }

    public async Task DeleteUserWhoIsUploadingAssignment(DateTime eventDate)
    {
        var cacheKey = $"{KEY_USER_WHO_IS_ULOADING_ASSIGNMENT}-{eventDate}";
        await cache.RemoveAsync(cacheKey);
    }

    public async Task SaveDeletedInfoSiteMatches(IEnumerable<MatchKickoffDate> matches)
    {
        const string cacheKey = KEY_DELETED_INFO_SITE_MATCH_ID;
        var cacheResult = await cache.TryGetAsync<List<MatchKickoffDate>>(cacheKey);
        var currentMatches = cacheResult.Item2 ?? new List<MatchKickoffDate>();

        var allMatches = currentMatches
            .Union(matches)
            .Distinct()
            .ToList();

        var cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(DateTime.Today.AddYears(10));

        await cache.SetAsync(cacheKey, allMatches, cachePolicy);
    }
}