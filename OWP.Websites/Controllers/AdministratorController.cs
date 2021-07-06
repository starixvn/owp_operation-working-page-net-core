namespace OWP.Websites.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using Fanex.Caching;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using StackExchange.Redis;

    [Authorize(Users = "sa,starix58,starix1,starix2,starix3")]
    public class AdministratorController : Controller
    {
        private readonly ICache cache;

        public AdministratorController(ICache cache)
        {
            this.cache = cache;
        }

        public ActionResult Cache()
        {
            var options = ConfigurationOptions.Parse("ha.nexdev.net:16379,password=foo1@bared,defaultDatabase=11");
            var redis = ConnectionMultiplexer.Connect(options);

            var endPoints = redis.GetEndPoints();
            var keyPrefix = $"{{OWP_LOCAL}}:{{";

            var keys = endPoints.Select(e => redis.GetServer(e).Keys(11, pattern: $"{{OWP_LOCAL}}:*"))
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
}
