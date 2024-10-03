using Fanex.NAP.Client;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OWP.NotificationSetting.Models;
namespace OWP.NotificationSetting;

[Route("api/notification-setting")]
public class NotificationController : ControllerBase
{
    private readonly Repository repository;
    private readonly INapSession session;

    public NotificationController(Repository repository, INapSession session)
    {
        this.repository = repository;
        this.session = session;
    }

    [Route("me")]
    public async Task<IActionResult> GetByCurrentUser()
    {
        var data = await repository.GetNotificationSettingByUserId(session.UserId);
        var assignees = data.Where(x => !x.IsWatcher).ToList();
        var watchers = data.Where(x => x.IsWatcher).ToList();
        var listNotifications = new NotificationSettingViewModel
        {
            Assignees = assignees,
            Watchers = watchers
        };
        return Ok(JsonConvert.SerializeObject(listNotifications)); 
    }

    [Route("update")]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] NotificationSettingUpdateModel model)
    {
        model.UserId = session.UserId;
        await repository.Update(model);

        return Ok(JsonConvert.SerializeObject(model)); 
    }
}