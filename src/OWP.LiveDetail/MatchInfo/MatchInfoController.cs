using System;
using System.Globalization;

namespace OWP.LiveDetail.MatchInfo
{
    using Common;
    using Constants;
    using Fanex.NAP.Client.AspNetCore.Attributes;
    using Fanex.NAP.Core.Business;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using OWP.Extensions;
    using Services;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Route("api/live-detail/match-info")]
    [ApiController]
    //[AllowAnonymous]
    [Authorize]
    [EnableCors("AllowOrigin")]
    public class MatchInfoController : ControllerBase
    {
        private readonly MatchInfoRepository repository;

        //private readonly NotificationService notificationService;

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
            GetAdminSiteMatchIdService getAdminSiteMatchIdService
        )
        //NotificationService notificationService,
        //RabbitMqClient rabbitMqClient)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            this.repository = repository;
            //this.notificationService = notificationService;
            this.permissionService = permissionService;
            this.cacheManipulator = cacheManipulator;
            this.checkIsLiveStatusServices = checkIsLiveStatusServices;
            this.getAdminSiteMatchIdService = getAdminSiteMatchIdService;
            //this.rabbitMqClient = rabbitMqClient;
        }

        [HttpGet]
        [Route("")]
        [NapAuth(FunctionKey = FunctionKeys.DetailsInformation, Policy = Constant.Policy.Read)]
        public async Task<MatchInfoModel> Query(int owpMatchId)
        {
            var matchInfo = await repository.Query(owpMatchId);
            if (matchInfo == null)
            {
                return null;
            }

            if (!permissionService.HasRight(FunctionKeys.FullTimeView, Constant.Policy.Read))
            {
                var matchTimeRangeResult = TimeRangeExtensions.CheckMainMatch(matchInfo.EventDate);
                if (!matchTimeRangeResult.IsAllowedToViewDetail)
                {
                    return null;
                }
            }

            matchInfo.TVs = await repository.GetTelevisionsByMatchId(owpMatchId);
            matchInfo.OtherProviders = await repository.GetOtherProvidersByMatchId(owpMatchId);
            matchInfo.Providers = await repository.GetProvidersByMatchId(owpMatchId);
            matchInfo.Agents = await repository.GetAgentsByMatchId(owpMatchId);
            matchInfo.MatchStatusList = await repository.GetMatchStatusByMatchId(owpMatchId);
            matchInfo.MatchOffers = await repository.GetMatchOfferByMatchId(owpMatchId);
            var noOfHDPs = await repository.GetAllByRefName("NoOfHDP");
            matchInfo.NoOfHDPs = GetNoOfHDPsWithSelected(matchInfo.NoOfHDPId, noOfHDPs);
            matchInfo.AdminSiteId = await getAdminSiteMatchIdService.GetAdminSiteMatchId(matchInfo.OWPMatchId,
                matchInfo.ParentMatchId ?? matchInfo.OWPMatchId, matchInfo.MinorBettypeId);

            return matchInfo;
        }

        private static IEnumerable<RefResourceModel> GetNoOfHDPsWithSelected(int? noOfHDP, IEnumerable<RefResourceModel> noOfHDPs)
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
            var sessionId = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);
            await cacheManipulator.SaveOldAssignmentSnapshot(sessionId, oldSnapshot);

            var updateResult = await repository.UpdateMatchInfo(model);
            if (updateResult.Code == 0)
            {
                //await notificationService.Notify(updateResult.Data as string);

#if !DEBUG
               /*  var request = new LinkedMatchNotifyRequest
                {
                    SessionId = sessionId,
                    EventDate = filterEventDate
                }; */
                //rabbitMqClient.Publish(request);
#endif
            }

            return Ok(updateResult);
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
                return Ok(new ResultDataObject
                {
                    Code = 1,
                    Message = validateResult.FailedMessage
                });
            }

            var updateResult = await repository.UpdateProviderInfo(model);
            if (updateResult.IsSuccess)
            {
                //await notificationService.Notify(updateResult.Data as string);
            }

            return Ok(updateResult);
        }

        [HttpPut]
        [Route("trading-info")]
        [NapAuth(FunctionKey = FunctionKeys.DetailsInformation, Policy = Constant.Policy.Host)]
        public async Task<IActionResult> UpdateTradingInfo(UpdateTradingInfoModel model)
        {
            var updateResult = await repository.UpdateTradingInfo(model);
            if (updateResult.IsSuccess)
            {
                //await notificationService.Notify(updateResult.Data as string);
            }

            return Ok(updateResult);
        }

        [HttpGet]
        [Route("validate-infositeId")]
        [NapAuth(FunctionKey = FunctionKeys.DetailsInformation, Policy = Constant.Policy.Read)]
        public async Task<ValidationInfositeIdModel> ValidateInfoSiteMatchId(int matchId, int infoSiteMatchId)
        {
            var match = await repository.GetMatchInfoForCheckIsLive(matchId, infoSiteMatchId);
            var mainDBResult = await checkIsLiveStatusServices.GetOfferLiveStatusFromMainDB(match.ParentAdminSiteId.ToString());

            var isLive = checkIsLiveStatusServices.GetOfferLiveStatus(
                match.MinorBettypeId,
                match.ParentAdminSiteId,
                mainDBResult);

            return new ValidationInfositeIdModel
            {
                IsInfoSiteLinked = match.IsInfoSiteLinked,
                IsLiveStatusSynchronized = match.IsLiveMatchStatus && isLive
            };
        }

        [HttpPost]
        [Route("get-admin-site-id")]
        [AllowAnonymous]
        public async Task<int?> GetAdminSiteMatchId(GetAdminSiteMatchIdRequest model)
        {
            return await getAdminSiteMatchIdService.GetAdminSiteMatchId(model.OWPMatchId, model.ParentMatchId,
                model.MinorBettypeId);
        }
    }
}