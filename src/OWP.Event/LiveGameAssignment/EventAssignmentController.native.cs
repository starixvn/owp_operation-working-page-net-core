using OWP.Services;

namespace OWP.Event.LiveGameAssignment;

using Constants;
using Fanex.NAP.Client.Attributes;
using Fanex.NAP.Client.Helpers.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using OWP.Event.LiveGameAssignment.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/event-assignment")]
[ApiController]
//[EnableCors]
[AllowAnonymous]
public class EventAssignmentController : ControllerBase
{
    private readonly LiveGameAssignmentRepository repository;
    //private readonly NotificationService notificationService;
    private readonly EventAssignmentPermission eventAssignmentPermission;
    private readonly GetAdminSiteMatchIdService getAdminSiteMatchIdService;
    private readonly SyncOffVRService syncOffVrService;

    public EventAssignmentController(LiveGameAssignmentRepository repository,
        //NotificationService notificationService,
        EventAssignmentPermission eventAssignmentPermission,
        GetAdminSiteMatchIdService getAdminSiteMatchIdService,
        SyncOffVRService syncOffVrService)
    {
        this.repository = repository;
        //this.notificationService = notificationService;
        this.eventAssignmentPermission = eventAssignmentPermission;
        this.getAdminSiteMatchIdService = getAdminSiteMatchIdService;
        this.syncOffVrService = syncOffVrService;
    }

    [HttpPost("")]
    //[NapAuth(FunctionKey = FunctionKeys.LiveGameAssignment, Policy = Constant.Policy.Read)]
    public async Task<IEnumerable<EventModel>> Query(EventQueryModel queryModel)
    {
        var permission = eventAssignmentPermission.BuildPermission();
        queryModel.FullTimeViewPermission = permission.FullTimeView;

        var eventModels = await repository.Query(queryModel);
        var parentMatchIds = string.Join(",", eventModels.Select(x => x.ParentMatchId).Distinct());
        var infoSiteTimeResult = await repository.GetInfoSiteTime(parentMatchIds);

        await repository.DetectEventHasTicket(eventModels);

        if (queryModel.ShowUnreadMessage)
        {
            eventModels = eventModels.Where(e => e.IsRead.HasValue && !e.IsRead.Value)?.ToList();
        }

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
            return eventModels;
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

        return eventModels;
    }

    [Route("add-event")]
    [HttpPost]
    [NapAuth(FunctionKey = FunctionKeys.LiveGameAssignment, Policy = Constant.Policy.Host)]
    public async Task<IActionResult> AddEvent(AddEventViewModel model)
    {
        var result = await repository.AddEvent(model);
        return Ok(result);
    }

    [Route("validate-to-edit-event")]
    [HttpPost]
    [NapAuth(FunctionKey = FunctionKeys.LiveGameAssignment, Policy = Constant.Policy.Host)]
    public async Task<IActionResult> ValidateToEditEvent(EditMatchViewModel model)
    {
        var result = await repository.ValidateToEditEvent(model);
        return Ok(result);
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
            //await notificationService.Notify(sessionId);
        }

        return Ok(result);
    }



    [Route("sync-off-vr")]
    [HttpPost]
    [NapAuth(FunctionKey = FunctionKeys.LiveGameAssignment, Policy = Constant.Policy.Host)]
    public async Task<IActionResult> SyncOffVR(Model model)
    {
        await syncOffVrService.Sync(model.date);

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

public class Model
{
    public DateTime date { get; set; }
}