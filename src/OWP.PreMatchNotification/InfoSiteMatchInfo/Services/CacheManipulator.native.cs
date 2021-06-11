namespace OWP.PreMatchNotification.InfoSiteMatchInfo.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Caching;
    using Newtonsoft.Json;
    using OWP.PreMatchNotification.InfoSiteMatchInfo.Models;

    public interface CacheManipulator
    {
        Task<IList<InfoSiteMatch>> GetOldInfoSiteMatchSnapshot(string sessionId);

        Task DeleteOldInfoSiteMatchSnapshot(string sessionId);
    }

    public class CacheManipulatorImp : CacheManipulator
    {
        private readonly ICache cache;

        private const string OLD_INFOSITEMATCHES_SNAPSHOT_KEY = "OWP-Old-InfoSiteMatches";

        public CacheManipulatorImp(ICache cache)
        {
            this.cache = cache;
        }

        public async Task<IList<InfoSiteMatch>> GetOldInfoSiteMatchSnapshot(string sessionId)
        {
            var cacheKey = $"{OLD_INFOSITEMATCHES_SNAPSHOT_KEY}-{sessionId}";
            (_, var value) = await cache.TryGetAsync<string>(cacheKey);
            var assignmentSnapshot = string.IsNullOrEmpty(value)
                ? new List<InfoSiteMatch>()
                : JsonConvert.DeserializeObject<List<InfoSiteMatch>>(value);

            return assignmentSnapshot ?? new List<InfoSiteMatch>();
        }

        public async Task DeleteOldInfoSiteMatchSnapshot(string sessionId)
        {
            var cacheKey = $"{OLD_INFOSITEMATCHES_SNAPSHOT_KEY}-{sessionId}";
            await cache.RemoveAsync(cacheKey);
        }
    }
}