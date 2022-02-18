namespace OWP.OtherSport.Submit.Services
{
    using Fanex.Caching;
    using Newtonsoft.Json;
    using OWP.OtherSport.Submit.Models;
    using OWP.OtherSport.Validation.Models;
    
    
    

    public interface CacheManipulator
    {
        Task<List<AssignmentModel>> GetNewAssignments(string sessionId);

        Task SaveOldAssignmentSnapshot(AssignmentSnapshot assignmentSnapshot, string sessionId);

        Task<string> GetUserWhoIsUploadingAssignment(DateTime eventDate);

        Task SaveUserWhoIsUploadingAssignment(List<DateTime> eventDates, string username);

        Task DeleteUserWhoIsUploadingAssignment(List<DateTime> eventDates);

        Task SaveLastUpdate(LastUpdateInfo info);

        Task<LastUpdateInfo> GetLastUpdate();
    }

    public class CacheManipulatorImp : CacheManipulator
    {
        private const string KEY_OS_ASSIGNMENT = "OWP-OS-AssignmentUpload";
        private const string KEY_OLD_OS_ASSIGNMENT_SNAPSHOT = "OWP-Old-OS-Assignment";
        private const string KEY_USER_WHO_IS_OS_UPLOADING_ASSIGNMENT = "OWP-User-Who-Is-OS-Uploading-Assignemnt";
        private const int CACHE_EXPIRED_IN_DAYS = 36500;
        private const int CACHE_USER_WHO_IS_ULOADING_ASSIGNMENT_EXPIRED_IN_MINUTE = 30;
        private readonly ICache cache;
        private const string KEY_OS_UPLOAD_LAST_UPDATED = "OWP-OS-AssignmentUpload-Last-Updated";

        public CacheManipulatorImp(ICache cache)
        {
            this.cache = cache;
        }

        public async Task<List<AssignmentModel>> GetNewAssignments(string sessionId)
        {
            var cacheKey = $"{KEY_OS_ASSIGNMENT}-{sessionId}";
            var (_, value) = await cache.TryGetAsync<string>(cacheKey);

            var assignments = string.IsNullOrEmpty(value)
                ? new List<AssignmentModel>()
                : JsonConvert.DeserializeObject<List<AssignmentModel>>(value);

            return assignments ?? new List<AssignmentModel>();
        }

        public async Task SaveOldAssignmentSnapshot(AssignmentSnapshot assignmentSnapshot, string sessionId)
        {
            var cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(365));
            var assignmentSnapshotInJson = JsonConvert.SerializeObject(assignmentSnapshot);
            var cacheKey = $"{KEY_OLD_OS_ASSIGNMENT_SNAPSHOT}-{sessionId}";
            await cache.SetAsync(cacheKey, assignmentSnapshotInJson, cachePolicy);
        }

        public async Task<string> GetUserWhoIsUploadingAssignment(DateTime eventDate)
        {
            var cacheKey = $"{KEY_USER_WHO_IS_OS_UPLOADING_ASSIGNMENT}-{eventDate}";
            var (_, value) = await cache.TryGetAsync<string>(cacheKey);

            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }

            return string.Empty;
        }

        public async Task SaveLastUpdate(LastUpdateInfo info)
        {
            var cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(CACHE_EXPIRED_IN_DAYS));
            var assignmentSnapshotInJson = JsonConvert.SerializeObject(info);
            await cache.RemoveAsync(KEY_OS_UPLOAD_LAST_UPDATED);
            await cache.SetAsync(KEY_OS_UPLOAD_LAST_UPDATED, assignmentSnapshotInJson, cachePolicy);
        }

        public async Task<LastUpdateInfo> GetLastUpdate()
        {
            var (_, value) = await cache.TryGetAsync<string>(KEY_OS_UPLOAD_LAST_UPDATED);

            var lastUpdated = string.IsNullOrEmpty(value)
                ? new LastUpdateInfo()
                : JsonConvert.DeserializeObject<LastUpdateInfo>(value);

            return lastUpdated;
        }

        public async Task SaveUserWhoIsUploadingAssignment(List<DateTime> eventDates, string username)
        {
            foreach (var eventDate in eventDates)
            {
                var cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CACHE_USER_WHO_IS_ULOADING_ASSIGNMENT_EXPIRED_IN_MINUTE));
                var cacheKey = $"{KEY_USER_WHO_IS_OS_UPLOADING_ASSIGNMENT}-{eventDate}";
                await cache.SetAsync(cacheKey, username, cachePolicy);
            }
        }

        public async Task DeleteUserWhoIsUploadingAssignment(List<DateTime> eventDates)
        {
            foreach (var eventDate in eventDates)
            {
                var cacheKey = $"{KEY_USER_WHO_IS_OS_UPLOADING_ASSIGNMENT}-{eventDate}";
                await cache.RemoveAsync(cacheKey);
            }
        }
    }
}