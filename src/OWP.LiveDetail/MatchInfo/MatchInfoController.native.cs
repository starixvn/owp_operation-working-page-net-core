using Fanex.NAP.Client.AspNetCore.Attributes;
using Fanex.NAP.Core.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OWP.Constants;
using OWP.Extensions;
using OWP.LiveDetail.Common;
using OWP.Models;
using OWP.RabbitMQ;
using OWP.RabbitMQ.Constants;
using OWP.Services;

namespace OWP.LiveDetail.MatchInfo;

[Route("api/live-detail/match-info")]
[ApiController]
[EnableCors("AllowOrigin")]
[Authorize]
public class MatchInfoController : ControllerBase
{
    private readonly MatchInfoRepository repository;

    private readonly MessageClient messageClient;

    private readonly PermissionService permissionService;

    private readonly LiveDetailCacheManipulator cacheManipulator;

    private readonly CheckIsLiveStatusServices checkIsLiveStatusServices;

    private readonly GetAdminSiteMatchIdService getAdminSiteMatchIdService;


#pragma warning disable S107 // Methods should not have too many parameters
    public MatchInfoController(
        MatchInfoRepository repository,
        PermissionService permissionService,
        LiveDetailCacheManipulator cacheManipulator,
        CheckIsLiveStatusServices checkIsLiveStatusServices,
        GetAdminSiteMatchIdService getAdminSiteMatchIdService,
        MessageClient messageClient
    )
#pragma warning restore S107 // Methods should not have too many parameters
    {
        this.repository = repository;
        this.permissionService = permissionService;
        this.cacheManipulator = cacheManipulator;
        this.checkIsLiveStatusServices = checkIsLiveStatusServices;
        this.getAdminSiteMatchIdService = getAdminSiteMatchIdService;
        this.messageClient = messageClient;
    }

    [HttpGet]
    [Route("")]
    [NapAuth(FunctionKey = FunctionKeys.DetailsInformation, Policy = Constant.Policy.Read)]
    public async Task<IActionResult> Query(int owpMatchId)
    {
        var matchInfo = await repository.Query(owpMatchId);
        if (matchInfo == null)
        {
            return new EmptyResult();
        }

        if (!permissionService.HasRight(FunctionKeys.FullTimeView, Constant.Policy.Read))
        {
            var matchTimeRangeResult = TimeRangeExtensions.CheckMainMatch(matchInfo.EventDate);
            if (!matchTimeRangeResult.IsAllowedToViewDetail)
            {
                return new EmptyResult();
            }
        }

        matchInfo.TVs = await repository.GetTelevisionsByMatchId(owpMatchId);
        matchInfo.OtherProviders = await repository.GetOtherProvidersByMatchId(owpMatchId);
        matchInfo.Providers = await repository.GetProvidersByMatchId(owpMatchId);
        matchInfo.Agents = await repository.GetAgentsByMatchId(owpMatchId);
        matchInfo.MatchStatusList = await repository.GetMatchStatusByMatchId(owpMatchId);
        matchInfo.MatchOffers = await repository.GetMatchOfferByMatchId(owpMatchId);
        var noOfHDPs = await repository.GetAllByRefName("NoOfHDP");
        matchInfo.NoOfHDPs = GetNoOfHDPsWithSelected(matchInfo.NoOfHDPId, noOfHDPs.ToList());
        matchInfo.AdminSiteId = await getAdminSiteMatchIdService.GetAdminSiteMatchId(matchInfo.OWPMatchId,
            matchInfo.ParentMatchId ?? matchInfo.OWPMatchId, matchInfo.MinorBettypeId);

        return Ok(JsonConvert.SerializeObject(matchInfo));
    }

    private static IEnumerable<RefResourceModel> GetNoOfHDPsWithSelected(int? noOfHDP,
        List<RefResourceModel> noOfHDPs)
    {
        foreach (var item in noOfHDPs)
        {
            if (item.Id == noOfHDP)
            {
                item.IsSelected = true;
            }
        }
        return noOfHDPs;
    }

    [HttpPut]
    [Route("")]
    [NapAuth(FunctionKey = FunctionKeys.DetailsInformation, Policy = Constant.Policy.Host)]
    public async Task<IActionResult> UpdateMatchInfo(UpdateMatchInfoModel model)
    {
        var filterEventDate = MapFilterEventDate(model.KickOffTime);
        var oldSnapshot = await repository.QueryMatchesByDate(filterEventDate);
        var updateResult = await repository.UpdateMatchInfo(model);
        if (updateResult.Code == 0)
        {
            var sessionId = updateResult.Data;
            messageClient.SendMessage(new { SessionId = sessionId }, RoutingKey.LiveDetail);
            await cacheManipulator.SaveOldAssignmentSnapshot(sessionId, oldSnapshot);
            messageClient.SendMessage(new
            {
                SessionId = sessionId,
                EventDate = filterEventDate.Date
            }, RoutingKey.LinkedLgaMatch);
        }

        return Ok(JsonConvert.SerializeObject(updateResult));
    }

    public DateTime MapFilterEventDate(DateTime eventDateTime)
    {
        const int twelfthHour = 12;
        return eventDateTime.Hour < twelfthHour ? eventDateTime.AddDays(-1) : eventDateTime;
    }

    [HttpPut]
    [Route("provider-info")]
    [NapAuth(FunctionKey = FunctionKeys.DetailsInformation, Policy = Constant.Policy.Host)]
    public async Task<IActionResult> UpdateProviderInfo(UpdateProviderInfoModel model)
    {
        var validatePhoneModel = new ValidatePhoneModel
        {
            OWPMatchId = model.OWPMatchId,
            Phone = model.Phone,
            IsValidateAssignment = false
        };

        var validateResult = await repository.ValidatePhone(validatePhoneModel);

        if (validateResult.IsFailed)
        {
            return Ok(JsonConvert.SerializeObject(new ResultDataObject
            {
                Code = 1,
                Message = validateResult.FailedMessage
            }));
        }
        var updateResult = await repository.UpdateProviderInfo(model);

        if (updateResult.IsSuccess)
        {
            messageClient.SendMessage(new { SessionId = updateResult.Data }, RoutingKey.LiveDetail);
        }
        return Ok(JsonConvert.SerializeObject(updateResult));
    }

    [HttpPut]
    [Route("trading-info")]
    [NapAuth(FunctionKey = FunctionKeys.DetailsInformation, Policy = Constant.Policy.Host)]
    public async Task<IActionResult> UpdateTradingInfo(UpdateTradingInfoModel model)
    {
        var updateResult = await repository.UpdateTradingInfo(model);
        if (updateResult.IsSuccess)
        {
            messageClient.SendMessage(new { SessionId = updateResult.Data }, RoutingKey.LiveDetail);
        }
        return Ok(JsonConvert.SerializeObject(updateResult));
    }

    [HttpGet]
    [Route("validate-infositeId")]
    [NapAuth(FunctionKey = FunctionKeys.DetailsInformation, Policy = Constant.Policy.Read)]
    public async Task<IActionResult> ValidateInfoSiteMatchId(int matchId, int infoSiteMatchId)
    {
        var match = await repository.GetMatchInfoForCheckIsLive(matchId, infoSiteMatchId);
        var mainDBResult =
            await checkIsLiveStatusServices.GetOfferLiveStatusFromMainDB(match.ParentAdminSiteId.ToString());

        var isLive = checkIsLiveStatusServices.GetOfferLiveStatus(
            match.MinorBettypeId,
            match.ParentAdminSiteId,
            mainDBResult);
        var result = new ValidationInfositeIdModel
        {
            IsInfoSiteLinked = match.IsInfoSiteLinked,
            IsLiveStatusSynchronized = match.IsLiveMatchStatus && isLive
        };
        return Ok(JsonConvert.SerializeObject(result));
    }

    [HttpPost]
    [Route("get-admin-site-id")]
    public async Task<int?> GetAdminSiteMatchId(GetAdminSiteMatchIdRequest model)
    {
        return await getAdminSiteMatchIdService.GetAdminSiteMatchId(model.OWPMatchId, model.ParentMatchId,
            model.MinorBettypeId);
    }
}