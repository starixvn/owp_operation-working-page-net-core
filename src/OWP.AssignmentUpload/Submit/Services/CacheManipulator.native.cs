using Fanex.Caching;
using Newtonsoft.Json;
using OWP.AssignmentUpload.Submit.Models;
using OWP.AssignmentUpload.Validation.Models;

namespace OWP.AssignmentUpload.Submit.Services;

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
    private const string KEY_USER_WHO_IS_UPLOADING_ASSIGNMENT = "OWP-User-Who-Is-Uploading-Assignemnt";
    private const string KEY_ASSIGNMENT_SNAPSHOT = "OWP-ASSIGNMENT-SNAPSHOT";
    private const string KEY_DELETED_INFO_SITE_MATCH_ID = "OWP-DELETED-INFO-SITE-MATCH-ID";
    private const int CACHE_EXPIRED_IN_30_MINS = 30;
    private const int CACHE_USER_WHO_IS_UPLOADING_ASSIGNMENT_EXPIRED_IN_MINUTE = 30;

    public CacheManipulatorImp(ICache cache)
    {
        this.cache = cache;
    }

    public async Task SaveAssignmentSnapshot(string sessionId, AssignmentSnapshot assignmentSnapshot)
    {
        var cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CACHE_EXPIRED_IN_30_MINS));
        var assignmentSnapshotInJson = JsonConvert.SerializeObject(assignmentSnapshot);
        var cacheKey = $"{KEY_ASSIGNMENT_SNAPSHOT}-{sessionId}";
        await cache.SetAsync(cacheKey, assignmentSnapshotInJson, cachePolicy);
    }

    public async Task<List<AssignmentModel>> GetAssignments(string sessionId)
    {
        var cacheKey = $"{KEY_ASSIGNMENT}-{sessionId}";
        var cacheResult = await cache.TryGetAsync<string>(cacheKey);

        var assignments = string.IsNullOrEmpty(cacheResult.Item2)
            ? []
            : JsonConvert.DeserializeObject<List<AssignmentModel>>(cacheResult.Item2);

        return assignments ?? [];
    }

    public async Task DeleteAssignments(string sessionId)
    {
        var cacheKey = $"{KEY_ASSIGNMENT}-{sessionId}";
        await cache.RemoveAsync(cacheKey);
    }

    public async Task<string> GetUserWhoIsUploadingAssignment(DateTime eventDate)
    {
        var cacheKey = $"{KEY_USER_WHO_IS_UPLOADING_ASSIGNMENT}-{eventDate}";
        var cacheResult = await cache.TryGetAsync<string>(cacheKey);

        return cacheResult.Item2 ?? string.Empty;
    }

    public async Task SaveUserWhoIsUploadingAssignment(DateTime eventDate, string username)
    {
        var cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CACHE_USER_WHO_IS_UPLOADING_ASSIGNMENT_EXPIRED_IN_MINUTE));
        var cacheKey = $"{KEY_USER_WHO_IS_UPLOADING_ASSIGNMENT}-{eventDate}";
        await cache.SetAsync(cacheKey, username, cachePolicy);
    }

    public async Task DeleteUserWhoIsUploadingAssignment(DateTime eventDate)
    {
        var cacheKey = $"{KEY_USER_WHO_IS_UPLOADING_ASSIGNMENT}-{eventDate}";
        await cache.RemoveAsync(cacheKey);
    }

    public async Task SaveDeletedInfoSiteMatches(IEnumerable<MatchKickoffDate> matches)
    {
        var cacheResult = await cache.TryGetAsync<List<MatchKickoffDate>>(KEY_DELETED_INFO_SITE_MATCH_ID);
        var currentMatches = cacheResult.Item2 ?? [];

        var allMatches = currentMatches
            .Union(matches)
            .Distinct()
            .ToList();

        var cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(DateTime.Today.AddYears(10));

        await cache.SetAsync(KEY_DELETED_INFO_SITE_MATCH_ID, allMatches, cachePolicy);
    }
}