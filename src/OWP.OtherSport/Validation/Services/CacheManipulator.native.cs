namespace OWP.OtherSport.Validation.Services
{
    using Fanex.Caching;
    using Models;
    using Newtonsoft.Json;

    public interface CacheManipulator
    {
        Task SaveAssignmentsSnapshot(string sessionId, List<AssignmentModel> assignments);
    }

    public class CacheManipulatorImp : CacheManipulator
    {
        private readonly ICache cache;

        private const string KEY_OS_ASSIGNMENT = "OWP-OS-AssignmentUpload";
        private const int ASSIGNMENT_CACHE_EXPIRED_IN_HOURS = 1;

        public CacheManipulatorImp(
            ICache cache)
        {
            this.cache = cache;
        }

        public async Task SaveAssignmentsSnapshot(string sessionId, List<AssignmentModel> assignments)
        {
            var expiredDate = DateTime.Today.AddHours(ASSIGNMENT_CACHE_EXPIRED_IN_HOURS);
            var absoluteTimeSpan = expiredDate - DateTime.Now;
            var cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(absoluteTimeSpan);
            var cacheKey = $"{KEY_OS_ASSIGNMENT}-{sessionId}";

            var assignmentsJson = JsonConvert.SerializeObject(assignments);
            await cache.SetAsync(cacheKey, assignmentsJson, cachePolicy);
        }
    }
}