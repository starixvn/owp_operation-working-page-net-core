namespace OWP.OtherSport.Submit.Services
{
    using Fanex.Caching;
    using Newtonsoft.Json;
    using OWP.OtherSport.Submit.Models;
    using OWP.OtherSport.Validation.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface CacheManipulator
    {
        Task<List<AssignmentModel>> GetNewAssignments();

        Task SaveOldAssignmentSnapshot(AssignmentSnapshot assignmentSnapshot);

        Task<string> GetUserWhoIsUploadingAssignment(List<DateTime> eventDates);

        Task SaveUserWhoIsUploadingAssignment(List<DateTime> eventDates, string username);

        Task DeleteUserWhoIsUploadingAssignment(List<DateTime> eventDates);
    }

    public class CacheManipulatorImp : CacheManipulator
    {
        private const string KEY_OS_ASSIGNMENT = "OWP-OS-AssignmentUpload";
        private const string KEY_OLD_OS_ASSIGNMENT_SNAPSHOT = "OWP-Old-OS-Assignment";
        private const string KEY_USER_WHO_IS_OS_UPLOADING_ASSIGNMENT = "OWP-User-Who-Is-OS-Uploading-Assignemnt";
        private const int CACHE_EXPIRED_IN_DAYS = 1;
        private const int CACHE_USER_WHO_IS_ULOADING_ASSIGNMENT_EXPIRED_IN_MINUTE = 30;
        private readonly ICache cache;

        public CacheManipulatorImp(ICache cache)
        {
            this.cache = cache;
        }

        public async Task<List<AssignmentModel>> GetNewAssignments()
        {
            var cacheKey = $"{KEY_OS_ASSIGNMENT}";
            var (_, value) = await cache.TryGetAsync<string>(cacheKey);

            var assignments = string.IsNullOrEmpty(value)
                ? new List<AssignmentModel>()
                : JsonConvert.DeserializeObject<List<AssignmentModel>>(value);

            return assignments ?? new List<AssignmentModel>();
        }

        public async Task SaveOldAssignmentSnapshot(AssignmentSnapshot assignmentSnapshot)
        {
            var cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(CACHE_EXPIRED_IN_DAYS));
            var assignmentSnapshotInJson = JsonConvert.SerializeObject(assignmentSnapshot);
            var cacheKey = $"{KEY_OLD_OS_ASSIGNMENT_SNAPSHOT}";
            await cache.SetAsync(cacheKey, assignmentSnapshotInJson, cachePolicy);
        }

        public async Task<string> GetUserWhoIsUploadingAssignment(List<DateTime> eventDates)
        {
            foreach (var eventDate in eventDates)
            {
                var cacheKey = $"{KEY_USER_WHO_IS_OS_UPLOADING_ASSIGNMENT}-{eventDate}";
                var (_, value) = await cache.TryGetAsync<string>(cacheKey);

                if (!string.IsNullOrEmpty(value))
                {
                    return value;
                }
            }

            return string.Empty;
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