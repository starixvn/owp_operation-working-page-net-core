using Fanex.Caching;
using Newtonsoft.Json;
using StackExchange.Redis;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OWP.PreMatchAssignment.Models;

namespace OWP.Websites.Controllers;

public class AdministratorController : Controller
{
    private readonly ICache cache;

    private readonly IConfiguration configuration;


    public AdministratorController(ICache cache, IConfiguration configuration)
    {
        this.cache = cache;
        this.configuration = configuration;
    }

    public ActionResult Cache()
    {
        var connectionString = configuration.GetSection("AuthSettings:RedisConfig").Value;
        var options = ConfigurationOptions.Parse(connectionString);
        var redis = ConnectionMultiplexer.Connect(options);

        var endPoints = redis.GetEndPoints();
        var appName = configuration.GetSection("fanex.caching.redis:ApplicationName").Value;
        var keyPrefix = $"{{{appName}}}:{{";

        var keys = endPoints.Select(e => redis.GetServer(e).Keys(11, pattern: $"{{{appName}}}:*"))
            .SelectMany(k => k)
            .Select(k => k.ToString().Replace(keyPrefix, string.Empty))
            .Select(k => k.Remove(k.Length - 1))
            .Distinct()
            .OrderBy(k => k)
            .ToArray();

        ViewBag.CacheKeys = keys;
        return View("Cache");
    }

    public ActionResult DeleteCache(string cacheKey)
    {
        cache.Remove(cacheKey);

        return RedirectToAction("Cache");
    }

    public async Task<ActionResult> SaveCache(string cacheKey)
    {
        var testObject = new List<AssignmentSnapshot>{
            new AssignmentSnapshot{
                LeagueName = "Tyler-TestLeague",
                PreMatchLeagueId = 1,
                IsDay = true
            }       
        };
        var cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1));
        var assignmentSnapshotInJson = JsonConvert.SerializeObject(testObject);
        await cache.SetAsync(cacheKey, "TylerTestContent", cachePolicy);
        return RedirectToAction("Cache");
    }

    public ActionResult CacheContent(string cacheKey, string typeName = null)
    {
        string cacheContent;
        if (string.IsNullOrEmpty(typeName))
        {
            cache.TryGet(cacheKey, out cacheContent);
        }
        else
        {
            var type = Type.GetType(typeName);
            var method = typeof(ICache).GetMethod(nameof(ICache.TryGet));
            var generic = method.MakeGenericMethod(type);
            var parameters = new object[] { cacheKey, null };
            generic.Invoke(cache, parameters);

            cacheContent = JsonConvert.SerializeObject(parameters[1]);
        }



        ViewBag.CacheContent = cacheContent;
        return View("CacheContent");
    }
}
