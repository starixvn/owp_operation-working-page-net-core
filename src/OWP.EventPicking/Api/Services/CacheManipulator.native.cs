using Fanex.Caching;
using Newtonsoft.Json;

namespace OWP.EventPicking.Api.Services;

public interface CacheManipulator
{
    Task<List<EventMBSpread>> GetMBSpreadsByDate(DateTime eventDate);

    Task SaveMBSpreadsByDate(DateTime eventDate, List<EventMBSpread> data);

    Task<bool> Remove(DateTime eventDate);
}

public class CacheManipulatorImp : CacheManipulator
{
    private readonly ICache cache;

    private const string MBSpreadKey = "MBSpreadKey";
    private const int MBSpreadKey_Expired_Day = 1;

    public CacheManipulatorImp(ICache cache)
    {
        this.cache = cache;
    }

    public async Task<List<EventMBSpread>> GetMBSpreadsByDate(DateTime eventDate)
    {
        var cacheKey = $"{MBSpreadKey}-{eventDate:yyyy/MM/dd}";

        var cacheResult = await cache.TryGetAsync<string>(cacheKey);

        var MBSpread = string.IsNullOrEmpty(cacheResult.Item2)
            ? []
            : JsonConvert.DeserializeObject<List<EventMBSpread>>(cacheResult.Item2);
        return MBSpread ?? [];
    }

    public async Task<bool> Remove(DateTime eventDate)
    {
        var cacheKey = $"{MBSpreadKey}-{eventDate:yyyy/MM/dd}";
        await cache.RemoveAsync(cacheKey);
        return true;
    }

    public async Task SaveMBSpreadsByDate(DateTime eventDate, List<EventMBSpread> data)
    {
        var cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(MBSpreadKey_Expired_Day));
        var cacheKey = $"{MBSpreadKey}-{eventDate:yyyy/MM/dd}";
        var snapshotInJson = JsonConvert.SerializeObject(data);
        await cache.SetAsync(cacheKey, snapshotInJson, cachePolicy);
    }

    
}