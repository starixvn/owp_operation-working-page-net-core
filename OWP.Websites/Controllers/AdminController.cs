using Fanex.Caching;
using Newtonsoft.Json;
using OWP.Extensions;
using StackExchange.Redis;

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

        var keys = endPoints.Select(e => redis.GetServer(e).Keys(11, $"{{{appName}}}:*"))
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

        return Redirect("Cache");
    }

    public async Task<ActionResult> SaveLastDateForRegenerateReport(string functionName, DateTime lastDate)
    {
        var cacheKeyDict = new Dictionary<string, string> {
            { "la", "OWP_SumReportData_LAReport_LastDate" },
            { "trader", "OWP_SumReportData_TraderReport_LastDate" },
            { "prematch", "OWP_SumReportData_PrematchReport_LastDate" },
            { "overtime", "OWP_SumReportData_OvertimeReport_LastDate" },
            { "matchlive", "OWP_SumReportData_MatchLiveReport_LastDate" }
        };
        var cacheKey = cacheKeyDict.GetValueOrDefault(functionName, string.Empty);
        var cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1));

        await cache.SetAsync(cacheKey, lastDate.ToString("yyyy-MM-dd"), cachePolicy);
        return Redirect("Cache");
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

    public ActionResult GetLogFile()
    {
        var fullPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "App_Data",
            "Files");

        if (!Directory.Exists(fullPath))
        {
            Directory.CreateDirectory(fullPath);
        }

        var directory = new DirectoryInfo(fullPath);
        var files = directory.GetFiles("*.xlsx").OrderByDescending(p => p.CreationTime).ToArray();
        var fileInfos = new List<object>();

        foreach (var file in files)
        {
            fileInfos.Add(new
            {
                fileName = file.Name,
                createdTime = file.CreationTimeUtc.AddHours(8)
            });
        }

        ViewBag.Files = fileInfos;
        return View("GetLogFile");
    }

    public ActionResult DownloadFile(string fileName)
    {
        var filePath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "App_Data",
            "Files",
            fileName);
        var bytes = System.IO.File.ReadAllBytes(filePath);
        return File(bytes, "application/octet-stream", fileName);
    }

    public ActionResult DeleteOldLoggedFile()
    {
        var fullPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "App_Data",
            "Files");

        var files = Directory.GetFiles(fullPath);
        var fileCount = 0;
        foreach (var file in files)
        {
            var fi = new FileInfo(file);
            if (fi.LastAccessTime >= DateTime.Now.AddMonths(-3))
            {
                continue;
            }

            fileCount++;
            fi.Delete();
        }

        ViewBag.FileCount = fileCount;
        return View("DeleteOldLoggedFile");
    }

    public async Task<string> QueryDataFromUrl(string url)
    {
        var httpClient = new HttpClient();
        var response = await httpClient.GetAsync(url);

        return response.IsSuccessStatusCode
            ? await response.Content.ReadAsStringAsync()
            : string.Empty;
    }
}