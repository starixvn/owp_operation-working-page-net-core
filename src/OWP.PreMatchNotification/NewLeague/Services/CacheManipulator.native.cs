namespace OWP.PreMatchNotification.NewLeague.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Caching;
    using Newtonsoft.Json;
    using OWP.PreMatchNotification.NewLeague.Models;

    public interface CacheManipulator
    {
        Task<IList<PreMatchLeague>> GetOldPreMatchLeagueSnapshot(string sessionId);

        Task DeleteOldPreMatchLeagueSnapshot(string sessionId);
    }

    public class CacheManipulatorImp : CacheManipulator
    {
        private readonly ICache cache;

        private const string OLD_PREMATCHLEAGUES_SNAPSHOT_KEY = "OWP-Old-PreMatchLeagues";

        public CacheManipulatorImp(ICache cache)
        {
            this.cache = cache;
        }

        public async Task<IList<PreMatchLeague>> GetOldPreMatchLeagueSnapshot(string sessionId)
        {
            var cacheKey = $"{OLD_PREMATCHLEAGUES_SNAPSHOT_KEY}-{sessionId}";
            (_, var value) = await cache.TryGetAsync<string>(cacheKey);
            var assignmentSnapshot = string.IsNullOrEmpty(value)
                ? new List<PreMatchLeague>()
                : JsonConvert.DeserializeObject<List<PreMatchLeague>>(value);

            return assignmentSnapshot ?? new List<PreMatchLeague>();
        }

        public async Task DeleteOldPreMatchLeagueSnapshot(string sessionId)
        {
            var cacheKey = $"{OLD_PREMATCHLEAGUES_SNAPSHOT_KEY}-{sessionId}";
            await cache.RemoveAsync(cacheKey);
        }
    }
}