using OWP.Services;

namespace OWP.Event.LiveGameAssignment;

using Constants;
using Download.Service;
using Fanex.NAP.Client.Attributes;
using Fanex.NAP.Client.Helpers.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;
using OWP.RabbitMQ.Constants;
using RabbitMQ;
using Services;
using System.Linq;
using System.Threading.Tasks;

[Route("api/event-assignment")]
[ApiController]
[Authorize]
public class EventAssignmentController : ControllerBase
{
    private readonly LiveGameAssignmentRepository repository;
    private readonly MessageClient messageClient;
    private readonly EventAssignmentPermission eventAssignmentPermission;
    private readonly GetAdminSiteMatchIdService getAdminSiteMatchIdService;
    private readonly SyncOffVRService syncOffVrService;
    private readonly IEventAssignmentService service;

    public EventAssignmentController(LiveGameAssignmentRepository repository,
        MessageClient messageClient,
        EventAssignmentPermission eventAssignmentPermission,
        GetAdminSiteMatchIdService getAdminSiteMatchIdService,
        IEventAssignmentService service,
        SyncOffVRService syncOffVrService)
    {
        this.repository = repository;
        this.messageClient = messageClient;
        this.eventAssignmentPermission = eventAssignmentPermission;
        this.getAdminSiteMatchIdService = getAdminSiteMatchIdService;
        this.syncOffVrService = syncOffVrService;
        this.service = service;
    }

    [HttpPost("")]
    [NapAuth(FunctionKey = FunctionKeys.LiveGameAssignment, Policy = Constant.Policy.Read)]
    public async Task<IActionResult> Query(EventQueryModel queryModel)
    {
        var permission = eventAssignmentPermission.BuildPermission();
        queryModel.FullTimeViewPermission = permission.FullTimeView;

        var eventModels = await repository.Query(queryModel);
        var parentMatchIds = string.Join(",", eventModels.Select(x => x.ParentMatchId).Distinct());
        var infoSiteTimeResult = await repository.GetInfoSiteTime(parentMatchIds);

        await repository.DetectEventHasTicket(eventModels);

        foreach (var match in eventModels)
        {
            var infoSite = infoSiteTimeResult.FirstOrDefault(x => x.ParentMatchId == match.ParentMatchId);
            match.AdminSiteId = await getAdminSiteMatchIdService.GetAdminSiteMatchId(match.OWPMatchId,
            match.ParentMatchId, match.MinorBettypeId);
            match.InfoSiteTime = infoSite?.KickOffTime;
            match.InfoSiteTimeDisplay = infoSite?.KickOffTimeDisplay;
        }

        var permissionFullBetTypes = permission.LiveGameAssignment_FullBetTypes;
        if (permissionFullBetTypes.IsRead)
        {
            return Ok(JsonConvert.SerializeObject(eventModels));
        }

        eventModels.ToList().ForEach(eventModel =>
        {
            eventModel.Handicap = null;
            eventModel.Handicap1st = null;
            eventModel.OverUnder = null;
            eventModel.OverUnder1st = null;
            eventModel.Specific15Mins = null;
            eventModel.Specific10Mins = null;
        });

        return Ok(JsonConvert.SerializeObject(eventModels));
    }

    [Route("get-last-updated")]
    [HttpGet]
    [NapAuth(FunctionKey = FunctionKeys.LiveGameAssignment, Policy = Constant.Policy.Read)]
    public async Task<IActionResult> GetLastUpdate(DateTime date)
    {
        var result = await service.GetLastUpdated(date);
        return Ok(JsonConvert.SerializeObject(result));
    }

    [Route("add-event")]
    [HttpPost]
    [NapAuth(FunctionKey = FunctionKeys.LiveGameAssignment, Policy = Constant.Policy.Host)]
    public async Task<IActionResult> AddEvent(AddEventViewModel model)
    {
        var result = await repository.AddEvent(model);
        return Ok(JsonConvert.SerializeObject(result));
    }

    [Route("validate-to-edit-event")]
    [HttpPost]
    [NapAuth(FunctionKey = FunctionKeys.LiveGameAssignment, Policy = Constant.Policy.Host)]
    public async Task<IActionResult> ValidateToEditEvent(EditMatchViewModel model)
    {
        var result = await repository.ValidateToEditEvent(model);
        return Ok(JsonConvert.SerializeObject(result));
    }

    [Route("edit-event")]
    [HttpPost]
    [NapAuth(FunctionKey = FunctionKeys.LiveGameAssignment, Policy = Constant.Policy.Host)]
    public async Task<IActionResult> EditEvent(EditMatchViewModel model)
    {
        var result = await repository.EditEvent(model);

        if (result.IsSuccess)
        {
            var sessionId = result.Data as string;
            messageClient.SendMessage(new { SessionId = sessionId }, RoutingKey.LiveDetail);
        }

        return Ok(JsonConvert.SerializeObject(result));
    }

    [Route("sync-off-vr")]
    [HttpPost]
    [NapAuth(FunctionKey = FunctionKeys.LiveGameAssignment, Policy = Constant.Policy.Host)]
    public async Task<IActionResult> SyncOffVR(SyncOffVRRequest model)
    {
        await syncOffVrService.Sync(model.Date);

        return Ok();
    }

    [Route("get-sync-off-vr-last-updated")]
    [HttpGet]
    [NapAuth(FunctionKey = FunctionKeys.LiveGameAssignment, Policy = Constant.Policy.Read)]
    public async Task<(string, string)> GetSyncOffVRLastUpdate(DateTime date)
    {
        var result = await repository.GetSyncOffVRLastUpdate(date);

        return result;
    }
}