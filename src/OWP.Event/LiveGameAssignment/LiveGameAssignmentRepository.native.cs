using Fanex.NAP.Client;
using OWP.Enums;

namespace OWP.Event.LiveGameAssignment;

using Criteria;
using EventShared.Builder;
using Extensions;
using Fanex.Data.Repository;
using Models;
using OWP.Event.EventShared.Enum;
using OWP.Event.EventShared.Helper;
using OWP.Event.EventShared.Models;
using OWP.Models;
using OWP.Services;
using Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Utils;
using Assignment = Models.Assignment;
using MatchInfo = Models.MatchInfo;
using MatchNote = Models.MatchNote;
using MatchOffer = Models.MatchOffer;
using MatchSetting = Models.MatchSetting;
using Television = Models.Television;

public interface LiveGameAssignmentRepository
{
    Task<IList<EventModel>> Query(EventQueryModel queryModel);

    Task<ResultDataObject> AddEvent(AddEventViewModel model);

    Task<ResultDataObject> ValidateToAddEvent(AddEventViewModel model);

    Task<ResultDataObject> ValidateToEditEvent(EditMatchViewModel model);

    Task<ResultDataObject> EditEvent(EditMatchViewModel model);

    Task DetectEventHasTicket(IList<EventModel> eventModels);

    Task<IList<InfoSiteTimeModel>> GetInfoSiteTime(string ids);

    Task<List<OffVRModel>> GetSyncOffVRMatches(DateTime eventDate, DateTime syncTime);

    Task LogActionSyncOffVRMatches(DateTime eventDate, DateTime syncTime);

    Task<(string, string)> GetSyncOffVRLastUpdate(DateTime eventDate);
}

public class LiveGameAssignmentRepositoryImp : LiveGameAssignmentRepository
{
    private readonly IDynamicRepository dynamicRepository;

    private readonly INapSession session;

    private readonly CheckIsLiveStatusServices checkIsLiveStatusServices;

    public LiveGameAssignmentRepositoryImp(IDynamicRepository repository, INapSession session, CheckIsLiveStatusServices checkIsLiveStatusServices)
    {
        dynamicRepository = repository;
        this.session = session;
        this.checkIsLiveStatusServices = checkIsLiveStatusServices;
    }

    public async Task<ResultDataObject> ValidateToAddEvent(AddEventViewModel model)
    {
        var matchSettingBettypeIds = model.MatchSettings.Select(matchSetting => matchSetting.Bettype).ToList();
        var criteria = new ValidateToAddEventCriteria
        {
            EventDate = model.EventDate,
            KickOffTime = model.KOTime,
            MatchStatus = model.MatchStatus,
            AwayName = model.AwayName,
            HomeName = model.HomeName,
            LeagueName = model.LeagueName,
            MatchNo = model.MatchNo,
            InfoSiteMatchId = model.InfoSiteId,
            MatchSettingBettypeIds = string.Join(",", matchSettingBettypeIds)
        };

        await dynamicRepository.ExecuteAsync(criteria);
        var result = new ResultDataObject
        {
            Code = criteria.ErrCode,
            Message = criteria.ErrMsg,
            Data = new ValidateToAddEventModel
            {
                ExistedMatchId = criteria.ExistedMatchId,
                ParentMatchId = criteria.ParentMatchId,
                MinorBettypeId = criteria.MinorBettypeId,
                MatchSettingBettypeIds = criteria.MatchSettingBettypeIds
            }
        };

        if (result.IsSuccess && model.ParentMatchId.HasValue && model.ParentMatchId != criteria.ParentMatchId)
        {
            return new ResultDataObject
            {
                Code = (int)ErrorCode.Error,
                Message = "System error"
            };
        }

        return result;
    }

    public async Task<ResultDataObject> ValidateToEditEvent(EditMatchViewModel model)
    {
        var criteria = new ValidateToEditEventCriteria
        {
            OWPMatchId = model.OWPMatchId,
            KickOffTime = model.KOTime,
            AwayName = model.AwayName,
            HomeName = model.HomeName,
            LeagueName = model.LeagueName,
            MatchNo = model.MatchNo
        };

        await dynamicRepository.ExecuteAsync(criteria);
        var result = new ResultDataObject
        {
            Code = criteria.ErrCode,
            Message = criteria.ErrMsg,
            Data = new ValidateToEditEventModel
            {
                OWPMatchId = model.OWPMatchId,
                ParentMatchId = criteria.ParentMatchId,
                MinorBettypeId = criteria.MinorBettypeId
            }
        };

        return result;
    }

    public async Task<ResultDataObject> AddEvent(AddEventViewModel model)
    {
        var validateResult = await ValidateToAddEvent(model);
        if (!validateResult.IsSuccess)
        {
            return validateResult;
        }

        var validateResultData = validateResult.Data as ValidateToAddEventModel;
        if (validateResultData.ParentMatchId.HasValue)
        {
            var parentMatchId = validateResultData.ParentMatchId.Value;
            var minorBettypeId = validateResultData.MinorBettypeId.Value;
            return await AddEventWithInputMinorMatch(model, parentMatchId, minorBettypeId);
        }

        return await AddEventWithInputMainMatch(model);
    }

    public async Task<ResultDataObject> EditEvent(EditMatchViewModel model)
    {
        var validateResult = await ValidateToEditEvent(model);
        if (!validateResult.IsSuccess)
        {
            return validateResult;
        }

        var validateResultData = validateResult.Data as ValidateToEditEventModel;

        return validateResultData.ParentMatchId.HasValue
            ? await EditEventWithInputMainMatchToMinorMatch(model, validateResultData)
            : await EditEventWithInputMainMatch(model);
    }

    private async Task<ResultDataObject> EditEventWithInputMainMatch(EditMatchViewModel model)
    {
        var sessionId  = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);
        var criteria = new EditMainMatchCriteria
        {
            ActorId = session.UserId,
            MatchNo = model.MatchNo,
            AwayName = model.AwayName,
            HomeName = model.HomeName,
            KickOffTime = model.KOTime,
            LeagueName = model.LeagueName,
            OWPMatchId = model.OWPMatchId,
            SessionId = sessionId
        };

        await dynamicRepository.ExecuteAsync(criteria);
        return new ResultDataObject
        {
            Code = criteria.ErrCode,
            Message = criteria.ErrMsg,
            Data = sessionId
        };
    }

    private async Task<ResultDataObject> EditEventWithInputMainMatchToMinorMatch(EditMatchViewModel model, ValidateToEditEventModel validateToEditEventModel)
    {
        var sessionId = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);
        var criteria = new EditMainMatchToMinorMatchCriteria
        {
            ParentMatchId = validateToEditEventModel.ParentMatchId,
            MinorBettypeId = validateToEditEventModel.MinorBettypeId,
            ActorId = session.UserId,
            MatchNo = model.MatchNo,
            AwayName = model.AwayName,
            HomeName = model.HomeName,
            LeagueName = model.LeagueName,
            OWPMatchId = model.OWPMatchId,
            SessionId = sessionId
        };

        await dynamicRepository.ExecuteAsync(criteria);
        return new ResultDataObject
        {
            Code = criteria.ErrCode,
            Message = criteria.ErrMsg,
            Data = sessionId
        };
    }

    private async Task<ResultDataObject> AddEventWithInputMainMatch(AddEventViewModel model)
    {
        var mainMatchSettings = model.MatchSettings.Where(ms => ms.Bettype > 0).ToList();
        var xminsMatchSettings = model.MatchSettings.Where(ms => ms.Bettype == (int)MajorBetType.XMins).ToList();
        xminsMatchSettings.ForEach(ms => ms.Bettype = (int)MajorBetType.Handicap);
        var fmMatchSettings = model.MatchSettings.Where(ms => ms.Bettype == (int)MajorBetType.FastMarket).ToList();
        fmMatchSettings.ForEach(ms => ms.Bettype = (int)MajorBetType.Handicap);
        var aioMatchSettings = model.MatchSettings.Where(ms => ms.Bettype == (int)MajorBetType.AllInOne).ToList();
        aioMatchSettings.ForEach(ms => ms.Bettype = (int)MajorBetType.Handicap);
        var mainMatchResult = await AddMainMatch(model);
        var owpMatchId = (int)mainMatchResult.Data;
        model.MatchSettings = mainMatchSettings;
        await AddMatchOddsAndMatchSettings(owpMatchId, model);
        if (!mainMatchResult.IsSuccess)
        {
            return mainMatchResult;
        }

        var minorMatchSettings = new List<MinorMatchSettings>
        {
            new() {
                MinorBettypeId = (int) MinorBetType.Specific15Mins,
                MatchSettings = xminsMatchSettings
            },
            new() {
                MinorBettypeId = (int) MinorBetType.Specific10Mins,
                MatchSettings = xminsMatchSettings
            },
            new() {
                MinorBettypeId = (int) MinorBetType.FastMarket,
                MatchSettings = fmMatchSettings
            },
            new() {
                MinorBettypeId = (int) MinorBetType.AllInOne,
                MatchSettings = aioMatchSettings
            }
        };

        foreach (var minorMatchSetting in minorMatchSettings)
        {
            var minorMatchModel = new AddEventViewModel
            {
                EventDate = model.EventDate,
                KOTime = model.KOTime,
                MatchStatus = model.MatchStatus,
                MatchSettings = minorMatchSetting.MatchSettings,
                MatchOdds = []
            };
            var minorMatchResult = await AddMinorMatch(minorMatchModel, owpMatchId, minorMatchSetting.MinorBettypeId);
            if (!minorMatchResult.IsSuccess)
            {
                return minorMatchResult;
            }

            var minorMatchId = (int)minorMatchResult.Data;
            await AddMatchOddsAndMatchSettings(minorMatchId, minorMatchModel);
        }

        return mainMatchResult;
    }

    private async Task<ResultDataObject> AddEventWithInputMinorMatch(AddEventViewModel model, int parentMatchId, int minorBettypeId)
    {
        var minorMatchSettings = new List<MinorMatchSettings>
        {
            new() {
                MinorBettypeId = minorBettypeId,
                MatchSettings = model.MatchSettings.Where(ms => ms.Bettype > 0).ToList()
            }
        };
        if (minorBettypeId == (int)MinorBetType.TotalBooking || minorBettypeId == (int)MinorBetType.NoOfCorner)
        {
            var xminsMatchSettings = model.MatchSettings.Where(ms => ms.Bettype == (int)MajorBetType.XMins).ToList();
            xminsMatchSettings.ForEach(ms => ms.Bettype = (int)MajorBetType.Handicap);
            minorMatchSettings.Add(new MinorMatchSettings
            {
                MinorBettypeId = minorBettypeId == (int)MinorBetType.TotalBooking ? (int)MinorBetType.Specific15MinsTotalBookings : (int)MinorBetType.Specific15MinsCorners,
                MatchSettings = xminsMatchSettings
            });
        }

        ResultDataObject firstMinorMatchResult = null;
        for (var i = 0; i < minorMatchSettings.Count; i++)
        {
            var minorMatchSetting = minorMatchSettings[i];
            var minorMatchModel = model;
            if (i != 0)
            {
                minorMatchModel = new AddEventViewModel
                {
                    EventDate = model.EventDate,
                    KOTime = model.KOTime,
                    MatchStatus = model.MatchStatus,
                    MatchOdds = []
                };
            }

            minorMatchModel.MatchSettings = minorMatchSetting.MatchSettings;
            var minorMatchResult = await AddMinorMatch(minorMatchModel, parentMatchId, minorMatchSetting.MinorBettypeId);
            if (!minorMatchResult.IsSuccess)
            {
                return minorMatchResult;
            }

            var minorMatchId = (int)minorMatchResult.Data;
            await AddMatchOddsAndMatchSettings(minorMatchId, minorMatchModel);

            if (i == 0)
            {
                firstMinorMatchResult = minorMatchResult;
            }
        }

        return firstMinorMatchResult;
    }

    private async Task<ResultDataObject> AddMainMatch(AddEventViewModel model)
    {
        var criteria = new AddMainMatchCriteria
        {
            EventDate = model.EventDate,
            MatchStatus = model.MatchStatus,
            MatchNo = model.MatchNo,
            AwayName = model.AwayName,
            HomeName = model.HomeName,
            InfoSiteMatchId = model.InfoSiteId,
            KickOffTime = model.KOTime,
            OtherTime = model.OtherTime,
            LeagueName = model.LeagueName,
            Website1 = model.Website1,
            Website2 = model.Website2,
            Website3 = model.Website3,
            Website4 = model.Website4,
            ActorId = session.UserId,
            Channel = model.Channel,
            Division = model.Division,
            Phone = model.Phone,
            Remark = model.Remark,
            NoOfHDPId = model.NoOfHDPId,
            Round = model.Round,
            WB = model.WB,
            TVIds = model.TVIds,
            AgentIds = model.AgentIds,
            MatchOfferIds = model.MatchOfferIds,
            OtherProvider = model.OtherProvider,
            Provider = model.Provider
        };

        await dynamicRepository.ExecuteAsync(criteria);

        return new ResultDataObject
        {
            Code = criteria.ErrCode,
            Message = criteria.ErrMsg,
            Data = criteria.OWPMatchId
        };
    }

    private async Task<ResultDataObject> AddMinorMatch(AddEventViewModel model, int parentMatchId, int minorBettypeId)
    {
        var criteria = new AddMinorMatchCriteria
        {
            ParentMatchId = parentMatchId,
            MinorBettypeId = minorBettypeId,
            EventDate = model.EventDate,
            MatchStatus = model.MatchStatus,
            MatchNo = model.MatchNo,
            InfoSiteMatchId = model.InfoSiteId,
            KickOffTime = model.KOTime,
            OtherTime = model.OtherTime,
            Website1 = model.Website1,
            Website2 = model.Website2,
            Website3 = model.Website3,
            Website4 = model.Website4,
            ActorId = session.UserId,
            Channel = model.Channel,
            Division = model.Division,
            Phone = model.Phone,
            Remark = model.Remark,
            NoOfHDPId = model.NoOfHDPId,
            Round = model.Round,
            WB = model.WB,
            TVIds = model.TVIds,
            AgentIds = model.AgentIds,
            MatchOfferIds = model.MatchOfferIds,
            OtherProvider = model.OtherProvider,
            Provider = model.Provider
        };

        await dynamicRepository.ExecuteAsync(criteria);
        return new ResultDataObject
        {
            Code = criteria.ErrCode,
            Message = criteria.ErrMsg,
            Data = criteria.MinorMatchId
        };
    }

    private async Task AddMatchOddsAndMatchSettings(int matchId, AddEventViewModel model)
    {
        if (matchId > 0)
        {
            var matchSettings = model.MatchSettings.Where(x => x.Spread.HasValue || x.Maxbet.HasValue).ToList();
            await AddMatchSetting(matchId, matchSettings);

            await AddRecommendOdds(matchId, model.MatchOdds);
        }
    }

    private async Task AddMatchSetting(int matchId, IEnumerable<Models.MatchSettingModel> models)
    {
        var dataXml = models.ToXmlString();
        var criteria = new AddMatchSettingCriteria
        {
            OWPMatchId = matchId,
            DataXml = dataXml
        };

        await dynamicRepository.ExecuteAsync(criteria);
    }

    private async Task AddRecommendOdds(int matchId, IEnumerable<MatchOddViewModel> models)
    {
        var criteria = new AddMatchRecommendOddsCriteria
        {
            OWPMatchId = matchId,
            SessionId = string.Empty,

            UserId = session.UserId
        };
        foreach (var item in models)
        {
            item.RecommendOddsXml = item.RecommendOdds.ToXML().ToString(SaveOptions.DisableFormatting);
        }

        criteria.DataXml = models.ToXmlString();


        await dynamicRepository.ExecuteAsync(criteria);
    }

    public async Task DetectEventHasTicket(IList<EventModel> eventModels)
    {
        var owpIds = string.Join(",", eventModels.Select(x => x.OWPMatchId));
        var hasTicketModel = await dynamicRepository.FetchAsync<HasTicketModel>(new DetectEventHasTicketCriteria
        {
            OWPMatchIds = owpIds,
            ActorId = session.UserId
        });

        foreach (var @event in eventModels)
        {
            var ticket = hasTicketModel.FirstOrDefault(x => x.ParentMatchId == @event.ParentMatchId);

            if (ticket is not null)
            {
                @event.HasTicket = ticket.HasTicket;
                @event.IsRead = ticket.IsRead;
            }
        }
    }

    public async Task<IList<EventModel>> Query(EventQueryModel queryModel)
    {
        var criteria = new GetEventAssignmentCriteria
        {
            EventDate = queryModel.EventDate,
            MatchStatusIds = string.Join(",", queryModel.MatchStatusIds)
        };

        var reader = await dynamicRepository
            .FetchMultipleAsync<MatchInfo, Assignment, MatchNote, MatchSetting, Television, MatchOffer>(criteria);

        var matches = reader.Item1.ToList();
        var assignments = reader.Item2.ToList();
        var notes = reader.Item3.ToList();
        var matchSettings = reader.Item4.ToList();
        var televisions = reader.Item5.ToList();
        var matchOffers = reader.Item6.ToList();
        if (!queryModel.FullTimeViewPermission.IsRead)
        {
            matches = FilterMatchesByTimeRange(matches);
        }

        var events = await MappingInfoToEventModel(matches, assignments, notes, matchSettings, televisions, matchOffers);
        var eventsByUsers = FilterEventModelByUser(events, queryModel.UserIds);
        var eventsByMatchType = FilterEventByMatchType(eventsByUsers, queryModel.MatchType);
        var eventsByOffVR = FilterEventModelByOffVR(eventsByMatchType, queryModel.SelectedOffVRs);
        return FilterEventModelByLiveStatus(eventsByOffVR, queryModel.SyncLiveStatus);
    }

    private static IList<EventModel> FilterEventModelByOffVR(IList<EventModel> events, int?[] offVRs)
        => events
            .Where(ev => offVRs.Any(offVR => offVR == null
                ? ev.MatchOffers.All(x => !x.MatchOfferValue.IsContains("OFF VR"))
                : ev.MatchOffers.Any(x => offVR == x.Id)))
            .ToList();

    private static IList<EventModel> FilterEventModelByLiveStatus(IList<EventModel> events, int liveStatus)
    {
        if (liveStatus == (int)SyncLiveFilter.All)
        {
            return events;
        }

        return liveStatus == (int)SyncLiveFilter.Live
            ? events.Where(i => i.IsLiveStatusSynchronized).ToList()
            : events.Where(i => !i.IsLiveStatusSynchronized).ToList();
    }

    private static List<MatchInfo> FilterMatchesByTimeRange(List<MatchInfo> matches)
    {
        var dateTime = DateTimeOffset.Now.ToGmt8();
        var timeRange = dateTime.TimeRangeOfDate();
        var selectedMatches = new List<MatchInfo>();
        foreach (var match in matches)
        {
            var result = timeRange.CheckMatch(match.EventDateTime);
            if (result.IsMatchInTimeRange)
            {
                match.IsAllowedToViewDetail = result.IsAllowedToViewDetail;
                selectedMatches.Add(match);
            }
        }

        return selectedMatches;
    }

    private static IList<EventModel> FilterEventModelByUser(IList<EventModel> events, string userIds)
    {
        if (string.IsNullOrEmpty(userIds))
        {
            return events;
        }

        return events
            .Where(e => IsUserAssignedToEvent(userIds, e))
            .ToList();
    }

    private static IList<EventModel> FilterEventByMatchType(IEnumerable<EventModel> matchInfos, string matchType)
    {
        var types = matchType.Split(',').ToList();
        if (IsSelectAllMatchType(types))
        {
            return matchInfos.ToList();
        }

        return types.Exists(x => x == EventAssignmentConst.MatchVirtual) ?
            matchInfos.Where(x => x.IsSaBa).ToList() :
            matchInfos.Where(x => !x.IsSaBa).ToList();
    }

    public static bool IsSelectAllMatchType(List<string> matchType)
        => matchType.Exists(x => x == EventAssignmentConst.MatchReal)
           && matchType.Exists(x => x == EventAssignmentConst.MatchVirtual);

    private static bool IsUserAssignedToEvent(string userIds, EventModel e)
    {
        var userIdsSplit = userIds.Split(",");
        var assignments = new List<IList<Assignment>>
        {
            e.OneXTwo,
            e.FM,
            e.AIO,
            e.Handicap,
            e.Handicap1st,
            e.OverUnder,
            e.OverUnder1st,
            e.OneXTwo1st,
            e.Specific15Mins,
            e.Specific10Mins,
            e.LA
        };

        foreach (var assignment in assignments)
        {
            if (assignment.Any(i => userIdsSplit.Any(userId => int.Parse(userId) == i.UserId)))
            {
                return true;
            }
        }

        return false;
    }

    private async Task<IList<EventModel>> MappingInfoToEventModel(IList<MatchInfo> matches,
        IList<Assignment> assignments,
        IList<MatchNote> notes,
        IList<MatchSetting> matchSettings,
        IList<Television> televisions,
        IList<MatchOffer> matchOffers)
    {
        var events = new List<EventModel>();

        var adminSiteIds = string.Join(",", matches.Select(x => x.ParentAdminSiteId).Distinct());
        var mainDBResult = await checkIsLiveStatusServices.GetOfferLiveStatusFromMainDB(adminSiteIds);

        foreach (var match in matches)
        {
            var matchOffer = GetMatchOffers(matchOffers, match.OWPMatchId);
            var eventInstance = new EventModel
            {
                OWPMatchId = match.OWPMatchId,
                ParentMatchId = match.ParentMatchId,
                MinorBettypeId = match.MinorBettypeId,
                MatchNo = match.MatchNo,
                MatchStatus = match.MatchStatus,
                NoOfHDP = match.NoOfHDP,
                NoOfHDPShort = MappingShortName.GetShortName(match.NoOfHDP),
                NoOfHDPId = match.NoOfHDPId,
                EventDateTime = match.EventDateTime,
                KickOffTime = match.KickOffTime.ToKOTime(),
                KickOffTimeStr = match.KickOffTime,
                OtherTime = match.OtherTime?.ToKOTime(),
                OtherTimeStr = match.OtherTime,
                LeagueGroup = match.LeagueGroup,
                League = match.League,
                Home = match.Home,
                Away = match.Away,
                MatchCode = match.MatchCode,
                TeamRound = match.TeamRound,
                Channel = TemplateIpTvChannelBuilder.Build(match.IpTv, match.Channel),
                Remark = match.Remark,
                Round = match.Round,
                Division = match.OutSide,
                OtherProvider = match.OtherProvider,
                Provider = match.Provider,
                Agent = match.Agent,
                WatchBet = match.WatchBet,
                Phone = match.Phone,
                AIOName = match.AIO,
                AIOShortName = MappingShortName.GetShortName(match.AIO),
                SOP = match.SOP,
                Notes = GetMatchNotes(notes, match.OWPMatchId),
                Televisions = GetTelevisions(televisions, match.OWPMatchId),
                MatchOffers = GetMatchOffers(matchOffers, match.OWPMatchId),
                MatchSettings = matchSettings.Where(x => x.OWPMatchId == match.OWPMatchId).ToList(),
                HandicapSetting = GetMatchSettings(matchSettings, match.OWPMatchId, (int)MajorBetType.Handicap),
                OverUnderSetting = GetMatchSettings(matchSettings, match.OWPMatchId, (int)MajorBetType.OverUnder),
                Handicap = GetAssignments(assignments, match.OWPMatchId, (int)MajorBetType.Handicap),
                Handicap1st = GetAssignments(assignments, match.OWPMatchId, (int)MajorBetType.Handicap1St),
                OverUnder = GetAssignments(assignments, match.OWPMatchId, (int)MajorBetType.OverUnder),
                OverUnder1st = GetAssignments(assignments, match.OWPMatchId, (int)MajorBetType.OverUnder1St),
                OneXTwo = GetAssignments(assignments, match.OWPMatchId, (int)MajorBetType.OneXTwo),
                OneXTwo1st = GetAssignments(assignments, match.OWPMatchId, (int)MajorBetType.OneXTwo1St),
                LA = GetAssignments(assignments, match.OWPMatchId, (int)MajorBetType.La2Nd),
                Website1 = match.Website1,
                Website2 = match.Website2,
                Website3 = match.Website3,
                Website4 = match.Website4,
                InfoSiteMatchId = match.InfoSiteMatchId,
                IsInfoSiteLinked = match.IsInfoSiteLinked,
                IsLiveStatusSynchronized = match.IsLiveMatchStatus &&
                    checkIsLiveStatusServices.GetOfferLiveStatus(match.MinorBettypeId, match.ParentAdminSiteId, mainDBResult),
                IsAllowedToViewDetail = match.IsAllowedToViewDetail,
                IPTVCopy = IPTVBuilder.Build(new IPTVModel
                {
                    Television = string.Join("/", GetTelevisions(televisions, match.OWPMatchId).Select(x => x.TVCode)),
                    IpTv = match.IpTv,
                    Channel = match.Channel,
                    Stream = match.WatchBet
                }),
                IsShowAbsenceIcon = match.IsShowAbsenceIcon
            };

            if (!match.IsMinorMatch)
            {
                eventInstance.Specific15Mins = GetAssignmentsForSpecialBettypeOfMainMatch(assignments, match, (int)MinorBetType.Specific15Mins);
                eventInstance.Specific10Mins = GetAssignmentsForSpecialBettypeOfMainMatch(assignments, match, (int)MinorBetType.Specific10Mins);
                eventInstance.FM = GetAssignmentsForSpecialBettypeOfMainMatch(assignments, match, (int)MinorBetType.FastMarket);
                eventInstance.AIO = GetAssignmentsForSpecialBettypeOfMainMatch(assignments, match, (int)MinorBetType.AllInOne);
            }
            else
            {
                eventInstance.Specific15Mins = GetAssignmentForSpecific15minsOfMinorMatch(assignments, match);
                eventInstance.FM = GetAssignmentForFMOfMinorMatch(assignments, match);
                eventInstance.AIO = GetAssignmentForAIOOfMinorMatch(assignments, match);
            }

            events.Add(eventInstance);
        }

        return events;
    }

    private static IList<Assignment> GetAssignments(IList<Assignment> assignments, int owpMatchId, int bettype)
    {
        return assignments.Where(x => x.OWPMatchId == owpMatchId && x.Bettype == bettype).ToList();
    }

    private static IList<Assignment> GetAssignmentsForSpecialBettypeOfMainMatch(IList<Assignment> assignments, MatchInfo matchInfo, int minorBettype)
    {
        return assignments.Where(x => x.MinorBettypeId == minorBettype
                                    && x.ParentMatchId == matchInfo.OWPMatchId
                                    && x.Bettype == (int)MajorBetType.Handicap).ToList();
    }

    private static IList<Assignment> GetAssignmentForSpecific15minsOfMinorMatch(IList<Assignment> assignments, MatchInfo matchInfo)
    {
        if (Equals(matchInfo.MinorBettypeId, (int)MinorBetType.NoOfCorner))
        {
            return assignments.Where(x => x.MinorBettypeId == (int)MinorBetType.Specific15MinsCorners
                                             && x.ParentMatchId == matchInfo.ParentMatchId
                                             && x.Bettype == (int)MajorBetType.Handicap).ToList();
        }

        if (Equals(matchInfo.MinorBettypeId, (int)MinorBetType.TotalBooking))
        {
            return assignments.Where(x => x.MinorBettypeId == (int)MinorBetType.Specific15MinsTotalBookings
                                              && x.ParentMatchId == matchInfo.ParentMatchId
                                              && x.Bettype == (int)MajorBetType.Handicap).ToList();
        }

        return new List<Assignment>();
    }

    private static IList<Assignment> GetAssignmentForAIOOfMinorMatch(IList<Assignment> assignments, MatchInfo matchInfo)
    {
        if (Equals(matchInfo.MinorBettypeId, (int)MinorBetType.NoOfCorner))
        {
            return assignments.Where(x => x.MinorBettypeId == (int)MinorBetType.AllInOneCorners
                                          && x.ParentMatchId == matchInfo.ParentMatchId
                                          && x.Bettype == (int)MajorBetType.Handicap).ToList();
        }

        if (Equals(matchInfo.MinorBettypeId, (int)MinorBetType.TotalBooking))
        {
            return assignments.Where(x => x.MinorBettypeId == (int)MinorBetType.AllInOneTotalBookings
                                          && x.ParentMatchId == matchInfo.ParentMatchId
                                          && x.Bettype == (int)MajorBetType.Handicap).ToList();
        }

        return new List<Assignment>();
    }

    private static IList<Assignment> GetAssignmentForFMOfMinorMatch(IList<Assignment> assignments, MatchInfo matchInfo)
    {
        if (Equals(matchInfo.MinorBettypeId, (int)MinorBetType.NoOfCorner))
        {
            return assignments.Where(x => x.MinorBettypeId == (int)MinorBetType.FastMarketCorners
                                          && x.ParentMatchId == matchInfo.ParentMatchId
                                          && x.Bettype == (int)MajorBetType.Handicap).ToList();
        }

        if (Equals(matchInfo.MinorBettypeId, (int)MinorBetType.TotalBooking))
        {
            return assignments.Where(x => x.MinorBettypeId == (int)MinorBetType.FastMarketTotalBookings
                                          && x.ParentMatchId == matchInfo.ParentMatchId
                                          && x.Bettype == (int)MajorBetType.Handicap).ToList();
        }

        return new List<Assignment>();
    }

    private static IList<MatchNote> GetMatchNotes(IList<MatchNote> notes, int owpMatchId)
    {
        return notes.Where(x => x.OWPMatchId == owpMatchId).ToList();
    }

    private static MatchSetting GetMatchSettings(IList<MatchSetting> matchSettings, int owpMatchId, int bettype)
    {
        return matchSettings.FirstOrDefault(x => x.OWPMatchId == owpMatchId && x.Bettype == bettype);
    }

    private static IList<Television> GetTelevisions(IList<Television> televisions, int owpMatchId)
    {
        return televisions.Where(x => x.OWPMatchId == owpMatchId).ToList();
    }

    private static IList<MatchOffer> GetMatchOffers(IList<MatchOffer> matchOffers, int owpMatchId)
    {
        var offers = matchOffers.Where(x => x.OWPMatchId == owpMatchId).ToList();
        
        // Populate MatchOfferShortName for each offer
        foreach (var offer in offers)
        {
            offer.MatchOfferShortName = MappingShortName.GetShortName(offer.MatchOfferValue);
        }
        
        return offers;
    }

    public async Task<IList<InfoSiteTimeModel>> GetInfoSiteTime(string ids)
    {
        var criteria = new GetInfoSiteTimeCriteria
        {
            ParentMatchIds = ids
        };
        return (await dynamicRepository.FetchAsync<InfoSiteTimeModel>(criteria)).ToList();
    }

    public async Task<List<OffVRModel>> GetSyncOffVRMatches(DateTime eventDate, DateTime syncTime)
    {
        var criteria = new SyncOffVRCriteria { EventDate = eventDate, SyncTime = syncTime };
        return (await dynamicRepository.FetchAsync<OffVRModel>(criteria)).ToList();
    }

    public async Task LogActionSyncOffVRMatches(DateTime eventDate, DateTime syncTime)
    {
        var criteria = new LogActionSyncOffVRCriteria
        {
            EventDate = eventDate,
            ActorId = session.UserId,
            SyncTime = syncTime
        };

        await dynamicRepository.ExecuteAsync(criteria);
    }

    public async Task<(string, string)> GetSyncOffVRLastUpdate(DateTime eventDate)
    {
        var criteria = new GetLastUpdatedSyncOffVRCriteria { EventDate = eventDate };
        var result = await dynamicRepository.FetchAsync<SyncOffVRInfo>(criteria);
        var first = result.FirstOrDefault();

        return (first?.SyncDateTime.ToString("MMM dd, hh:mm tt"), first?.Username);
    }
}

public class SyncOffVRInfo
{
    public DateTime SyncDateTime { get; set; }

    public string Username { get; set; }
}