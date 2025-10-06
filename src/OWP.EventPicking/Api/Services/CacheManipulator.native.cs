using Fanex.Caching;
using Newtonsoft.Json;

namespace OWP.EventPicking.Api.Services;

public interface CacheManipulator
{
    Task<List<EventMBSpread>> GetMBSpreadsByDate(DateTime eventDate);

    Task SaveMBSpreadsByDate(DateTime eventDate, List<EventMBSpread> data);

}

public class CacheManipulatorImp : CacheManipulator
{
    private readonly ICache cache;

    private const string MBSpreadKey = "OWP-AssignmentUpload";
    private const int MBSpreadKey_Expired_Day = 1;

    public CacheManipulatorImp(ICache cache)
    {
        this.cache = cache;
    }

    public async Task<List<EventMBSpread>> GetMBSpreadsByDate(DateTime eventDate)
    {
        var cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(MBSpreadKey_Expired_Day));
        var cacheKey = $"{MBSpreadKey}-{eventDate}";

        var cacheResult = await cache.TryGetAsync<string>(cacheKey);

        var MBSpread = string.IsNullOrEmpty(cacheResult.Item2)
            ? []
            : JsonConvert.DeserializeObject<List<EventMBSpread>>(cacheResult.Item2);
        return MBSpread ?? [];
    }

    public async Task SaveMBSpreadsByDate(DateTime eventDate, List<EventMBSpread> data)
    {
        var cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(MBSpreadKey_Expired_Day));
        var cacheKey = $"{MBSpreadKey}-{eventDate}";
        await cache.SetAsync(cacheKey, data, cachePolicy);
    }

    
}