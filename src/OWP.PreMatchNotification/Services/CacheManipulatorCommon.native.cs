using Fanex.Caching;
using Newtonsoft.Json;

namespace OWP.PreMatchNotification.Services
{
    public interface CacheManipulatorCommon
    {
        Task<IList<T>> GetMultipleData<T>(string key);

        Task<T> GetSingleData<T>(string key, T defaultValue);

        Task RemoveData(string key);
    }

    public class CacheManipulatorCommonImp : CacheManipulatorCommon
    {
        private readonly ICache cache;

        public CacheManipulatorCommonImp(ICache cache)
        {
            this.cache = cache;
        }

        public async Task<IList<T>> GetMultipleData<T>(string key)
        {
            (_, var value) = await cache.TryGetAsync<string>(key);
            var assignmentSnapshot = string.IsNullOrEmpty(value)
                ? new List<T>()
                : JsonConvert.DeserializeObject<List<T>>(value);

            return assignmentSnapshot ?? new List<T>();
        }

        public async Task<T> GetSingleData<T>(string key, T defaultValue)
        {
            (_, var value) = await cache.TryGetAsync<string>(key);
            var assignmentSnapshot = string.IsNullOrEmpty(value)
                ? defaultValue
                : JsonConvert.DeserializeObject<T>(value);

            return assignmentSnapshot ?? defaultValue;
        }

        public async Task RemoveData(string key)
            => await cache.RemoveAsync(key);
    }
}