namespace OWP.PreMatchNotification.LinkedMatch.Services
{
    using Fanex.Caching;
    using Models;
    using Newtonsoft.Json;
    using System;
    using System.Threading.Tasks;

    public interface CacheManipulator
    {
        Task<AssignmentSnapshot> GetOldLGAMatchSnapshot(string sessionId, DateTime eventDate);
    }

    public class CacheManipulatorImp : CacheManipulator
    {
        private readonly ICache cache;
        private const string OLD_LGAMATCHES_SNAPSHOT_KEY = "OWP-Old-LGAMatches";

        public CacheManipulatorImp(ICache cache)
        {
            this.cache = cache;
        }

        public async Task<AssignmentSnapshot> GetOldLGAMatchSnapshot(string sessionId, DateTime eventDate)
        {
            var cacheKey = $"{OLD_LGAMATCHES_SNAPSHOT_KEY}-{sessionId}-{eventDate}";
            var cacheResult = await cache.TryGetAsync<string>(cacheKey);
            var assignmentSnapshot = string.IsNullOrEmpty(cacheResult.Item2)
                ? new AssignmentSnapshot()
                : JsonConvert.DeserializeObject<AssignmentSnapshot>(cacheResult.Item2);

            return assignmentSnapshot ?? new AssignmentSnapshot();
        }
    }
}
