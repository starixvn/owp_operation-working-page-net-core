using Fanex.Logging;
using Fanex.NAP.Client.Helpers.Web;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OWP.Models;
using System.Text;

namespace OWP.Event.LiveGameAssignment.Services;

public interface SyncOffVRService
{
    Task Sync(DateTime date);
}

public class SyncOffVRServiceImp : SyncOffVRService
{
    private readonly LiveGameAssignmentRepository repository;
    private readonly INapSession session;
    private readonly AppSettings settings;
    private readonly static HttpClient httpClient = new();

    public SyncOffVRServiceImp(
        LiveGameAssignmentRepository repository,
        INapSession session,
        IOptions<AppSettings> options)
    {
        this.repository = repository;
        this.session = session;
        settings = options.Value;
    }

    public async Task Sync(DateTime date)
    {
        var now = DateTime.UtcNow.AddHours(8);
        var result = await repository.GetSyncOffVRMatches(date, now);
        await repository.LogActionSyncOffVRMatches(date, now);

        string json = CreateJsonBody(result);

        await PostSyncOffVRAsync(json, now);
    }

    private string CreateJsonBody(List<OffVRModel> result)
    {
        var model = new List<UpdateVirtualRateModel> {
            new () {
                UserId = session.UserId,
                VirtualRates = result
            }
        };
        var json = JsonConvert.SerializeObject(model);
        return json;
    }

    public async Task PostSyncOffVRAsync(string json, DateTime now)
    {
        const string path = "api/VirtualRate/UpdateEnableVirtualRate";
        var infositeDomain = settings.AdminSiteAPI;

        var requestUrl = $"{infositeDomain}{path}";
        var bodyContent = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var response = await httpClient.PostAsync(requestUrl, bodyContent);
            var responseText = await response.Content.ReadAsStringAsync();
            await LogInfo(requestUrl, json, responseText, now);
        }
        catch (Exception ex)
        {
            await LogError(requestUrl, json, ex, now);
        }
    }

    private static async Task LogError(string requestUrl, string body, Exception ex, DateTime now)
    {
        var logErrorString = JsonConvert.SerializeObject(new
        {
            DateTime = now,
            RequestUri = requestUrl,
            ex.Message,
            BodyContent = body
        });

        await Logger.Log.ErrorAsync($"Syn-Off-VR {logErrorString}");
    }

    private static async Task LogInfo(string requestUrl, string body, string response, DateTime now)
    {
        var logInfoString = JsonConvert.SerializeObject(new
        {
            DateTime = now,
            RequestUri = requestUrl,
            BodyContent = body,
            Response = response
        });

        await Logger.Log.InfoAsync($"Syn-Off-VR {logInfoString}");
    }
}

public class OffVRModel
{
    [JsonIgnore]
    public int OWPMatchId { get; set; }

    [JsonProperty("matchId")]
    public int AdminSiteMatchId { get; set; }

    [JsonIgnore]
    public string OffVR { get; set; }

    [JsonProperty("updateType")]
    public string UpdateType => OffVR switch
    {
        "OFF VR" => "OffVr",
        "OFF VR (Whole Time)" => "OffVrWholeTime",
        "OFF VR 10 mins" => "OffVr10Mins",
        "OFF VR HDP" => "OffVrHdp",
        "OFF VR HDP 10 mins" => "OffVrHdp10Mins",
        _ => "OnVr",
    };
}

public class UpdateVirtualRateModel
{
    [JsonProperty("userId")]
    public int UserId { get; set; }

    [JsonProperty("enableVirtualRates")]
    public List<OffVRModel> VirtualRates { get; set; }
}

public class SyncOffVRRequest
{
    public DateTime Date { get; set; }
}