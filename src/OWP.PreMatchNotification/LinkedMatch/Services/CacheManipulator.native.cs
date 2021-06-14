namespace OWP.PreMatchNotification.LinkedMatch.Services
{
    using Fanex.Caching;
    using Models;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface CacheManipulator
    {
        Task<List<GeneralMatch>> GetOldLGAMatchSnapshot(string sessionId, DateTime eventDate);

        Task DeleteAssignmentSnapshot(string sessionId, DateTime eventDate);
    }

    public class CacheManipulatorImp : CacheManipulator
    {
        private readonly ICache cache;
        private const string OLD_LGAMATCHES_SNAPSHOT_KEY = "OWP-Old-LGAMatches";

        public CacheManipulatorImp(ICache cache)
        {
            this.cache = cache;
        }

        public async Task<List<GeneralMatch>> GetOldLGAMatchSnapshot(string sessionId, DateTime eventDate)
        {
            var cacheKey = $"{OLD_LGAMATCHES_SNAPSHOT_KEY}-{sessionId}-{eventDate}";
            var cacheResult = await cache.TryGetAsync<string>(cacheKey);
            var assignmentSnapshot = string.IsNullOrEmpty(cacheResult.Item2)
                ? new AssignmentSnapshot()
                : JsonConvert.DeserializeObject<AssignmentSnapshot>(cacheResult.Item2);

            return assignmentSnapshot == null
                ? new List<GeneralMatch>()
                : assignmentSnapshot.Matches.Values.ToList();
        }

        public async Task DeleteAssignmentSnapshot(string sessionId, DateTime eventDate)
        {
            var cacheKey = $"{OLD_LGAMATCHES_SNAPSHOT_KEY}-{sessionId}-{eventDate}";
            await cache.RemoveAsync(cacheKey);
        }
    }
}
