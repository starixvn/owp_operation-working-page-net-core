using Fanex.Logging;
using Newtonsoft.Json;

namespace OWP.Event.LiveGameAssignment.Services;

public interface SyncOffVRService
{
    Task Sync(DateTime date);
}

public class SyncOffVRServiceImp : SyncOffVRService
{
    private readonly LiveGameAssignmentRepository repository;

    public SyncOffVRServiceImp(LiveGameAssignmentRepository repository)
    {
        this.repository = repository;
    }

    public async Task Sync(DateTime date)
    {
        var now = DateTime.UtcNow.AddHours(8);
        var result = await repository.GetSyncOffVRMatches(date, now);
        var json = JsonConvert.SerializeObject(result);

        await repository.LogActionSyncOffVRMatches(date, now);
        await Logger.Log.InfoAsync($"Syn-Off-VR: {date.Date}, syncTime: {now}, {json}");
    }
}

public class OffVRModel
{
    public int OWPMatchId { get; set; }
    public int AdminSiteMatchId { get; set; }
    public string OffVR { get; set; }
}

public class SyncOffVRRequest
{
    public DateTime date { get; set; }
}