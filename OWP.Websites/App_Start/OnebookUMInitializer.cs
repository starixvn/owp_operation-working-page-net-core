
using Fanex.Caching;
using ONEbook.UM;
using ONEbook.UM.AspNetCore;
using ONEbook.UM.AspNetCore.Filters;
using ONEbook.UM.Configurations;
using ONEbook.UM.Helpers;
using ONEbook.UM.Services;
using ONEbook.UM.Transport;

namespace OWP.Websites;

using Microsoft.Extensions.Caching.Memory;

public static class OnebookUMInitializer
{
    private static UMClientConfiguration umConfig;

    public static void InitializeUMClientManager(IConfiguration configuration)
    {
        umConfig = new UMClientConfiguration();
        umConfig.Initialize(configuration);
        UMClientManager
            .UseConfig(umConfig)
            .UseCacheService(new UMCacheService(new CacheService()))
            .UseRestClient(new UMRestClient(umConfig))
            .UseHttpContext(new HttpContextHelper());
    }

    public static void RegisterServiceCollection(IServiceCollection services)
    {
        services.AddSingleton(umConfig);
        services.AddSingleton<CheckUMFilterAttribute>();
        services.AddSingleton<IUMCacheService, MemoryUMCacheService>();
        services.AddSingleton<IUMRestClient, UMRestClient>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<IHttpContextHelper, HttpContextHelper>();
        services.AddSingleton<IMaintenanceService, MaintenanceService>();
    }
}

public class HttpContextHelper : IHttpContextHelper
{
    public string GetSession(string key)
    {
        return string.Empty;
    }

    public string GetValueFromQueryString(string queryKey)
    {
        return string.Empty;
    }

    public void SetSession(string key, string value)
    {
    }
}

public class MemoryUMCacheService : IUMCacheService
{
    private readonly IMemoryCache memoryCache;

    public MemoryUMCacheService(IMemoryCache memoryCache)
    {
        this.memoryCache = memoryCache;
    }

    public T Get<T>(string key) => memoryCache.Get<T>(key)
    ;

    public void Remove(string key)
    {
        memoryCache.Remove(key);
    }

    public void Set<T>(string key, T value, TimeSpan absoluteExpiration)
        => memoryCache.Set(key, value, new MemoryCacheEntryOptions().SetAbsoluteExpiration(absoluteExpiration));
}