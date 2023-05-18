using Fanex.NAP.Client.Helpers.Web;
using Fanex.NAP.Core.Business;
using OWP.Constants;
using OWP.Extensions;
using OWP.Helpers;
using OWP.LiveGameList.Builder;
using OWP.LiveGameList.Const;
using OWP.LiveGameList.Criteria;
using OWP.LiveGameList.Models;
using OWP.Services;
using System.Net;

namespace OWP.LiveGameList.Services;

public interface EventInformationService
{
    Task<IEnumerable<MatchInfo>> GetEvents(DateTime eventDate, string statusId);
}

public class EventInformationServiceImp : EventInformationService
{
    private readonly INapSession currentSession;
    private readonly GetAdminSiteMatchIdService getAdminSiteMatchIdService;
    private readonly PermissionService permissionService;
    private readonly Repository repository;
    private readonly GetInfoSiteTimeService getInfoSiteTimeService;
    private readonly GetAdminSiteInfoService getAdminSiteInfoService;

    private List<MatchInfo> matchInfos = new();

    public EventInformationServiceImp(
        Repository repository,
        INapSession currentSession,
        GetAdminSiteMatchIdService getAdminSiteMatchIdService,
        PermissionService permissionService, GetInfoSiteTimeService getInfoSiteTimeService, GetAdminSiteInfoService getAdminSiteInfoService)
    {
        this.repository = repository;
        this.currentSession = currentSession;
        this.getAdminSiteMatchIdService = getAdminSiteMatchIdService;
        this.permissionService = permissionService;
        this.getInfoSiteTimeService = getInfoSiteTimeService;
        this.getAdminSiteInfoService = getAdminSiteInfoService;
    }

    public async Task BuildInfoMatch()
    {
        var parentMatchIds = string.Join(',', matchInfos.Select(x => x.ParentMatchId).Distinct());

        var infoSiteInfo = (await getInfoSiteTimeService.GetInfoSiteTime(parentMatchIds)).ToList();

        var adminSiteIdsWithOwpMatches = (await getAdminSiteInfoService.GetAdminSiteMatchId(parentMatchIds)).ToList();

        var adminSitesIdsOdds = (await getAdminSiteInfoService.GetAdminSiteInfo()).ToList();

        var adminSiteMatchInfo = LiveGameHelper.MergeAdminSiteInfo(adminSiteIdsWithOwpMatches, adminSitesIdsOdds);

        foreach (var matchInfo in matchInfos)
        {
            var infoSite = infoSiteInfo.FirstOrDefault(x => x.ParentMatchId == matchInfo.ParentMatchId);
            var isShowAbsenceIcon = adminSiteMatchInfo.Any(x => x.ParentMatchId == matchInfo.OWPMatchId);
            matchInfo.InfoSiteTime = infoSite?.KickOffTime;
            matchInfo.InfoSiteTimeDisplay = infoSite?.KickOffTimeDisplay;
            matchInfo.IsShowAbsenceIcon = !LiveGameListConst.StatusNotOffered.Contains(matchInfo.MatchStatus) && isShowAbsenceIcon;
        }
    }

    public async Task<IEnumerable<MatchInfo>> GetEvents(DateTime eventDate, string statusId)
    {
        var (matchesRaw, assignments, matchSetting, televisions, matchOffers, tickets) = repository
            .GetEventInformation(
                new GetEventsCriteria
                {
                    EventDate = eventDate,
                    MatchStatusIds = statusId,
                    ActorId = currentSession.UserId,
                    ShowAll = IsShowFullEventInformation()
                });

        matchInfos = matchesRaw.ToList();
        BuildEvent(assignments, matchSetting, televisions, matchOffers, tickets.ToList());

        await getAdminSiteMatchIdService.UpdateMinorAdminMatchId(matchInfos);
        await BuildInfoMatch();

        return matchInfos;
    }

    private void BuildEvent(
        IEnumerable<Assignment> assignments,
        IEnumerable<MatchSetting> matchSettings,
        IEnumerable<Television> televisions,
        IEnumerable<MatchOffer> matchOffers,
        IList<TicketInfo> tickets)
    {
        var (assignmentsDict, minorAssignmentsDict) = GetAssignmentsInDict(assignments);
        var (matchSettingsDict, minorMatchSettingsDict) = GetMatchSettingsInDict(matchSettings);
        var televisionsDict = GetTelevisionsDict(televisions);
        var matchOffersDict = GetMatchOffersDict(matchOffers);

        foreach (var matchInfo in matchInfos)
        {
            var owpMatchId = matchInfo.OWPMatchId;
            var assignmentsOfMatch = assignmentsDict.GetValue(owpMatchId, new List<Assignment>());
            var generatedAssignments = minorAssignmentsDict.GetValue(owpMatchId, new List<Assignment>());
            var matchSettingsOfMatch = matchSettingsDict.GetValue(owpMatchId, new List<MatchSetting>());
            var generatedMatchSettings = minorMatchSettingsDict.GetValue(owpMatchId, new List<MatchSetting>());

            matchInfo.Assignment = AssignmentBuilder.Build(matchInfo, assignmentsOfMatch, generatedAssignments);
            matchInfo.MatchSetting = MatchSettingBuilder.Build(matchInfo, matchSettingsOfMatch, generatedMatchSettings);
            matchInfo.Televisions = televisionsDict.GetValue(owpMatchId, new List<Television>());
            matchInfo.MatchOffers = matchOffersDict.GetValue(owpMatchId, new List<MatchOffer>())
                .Select(x => new MatchOffer
                {
                    OWPMatchId = x.OWPMatchId,
                    MatchOfferValue = WebUtility.HtmlEncode(x.MatchOfferValue)
                });
            matchInfo.IPTVCopy = IPTVBuilder.Build(new IPTVModel
            {
                Television = string.Join("/", matchInfo.Televisions.Select(t => t.TVCode)),
                IpTv = matchInfo.IPTV,
                Channel = matchInfo.Channel,
                Stream = matchInfo.WatchBet
            });
            var ticketByMatchId = tickets
                .Where(x => x.OwpMatchId == matchInfo.OWPMatchId)
                .ToList();
            matchInfo.TicketInfos = ticketByMatchId.OrderByDescending(x => x.LatestCreatedDate);
            matchInfo.HasTicket = ticketByMatchId.Any();
            matchInfo.IsRead = ticketByMatchId.All(x => x.IsRead);

            LengthPropertyBuilder.BuildLengthProperty(matchInfo);
        }
    }

    private static Dictionary<int, List<Television>> GetTelevisionsDict(IEnumerable<Television> televisions)
        => televisions.GroupBy(a => a.OWPMatchId).ToDictionary(a => a.Key, a => a.ToList());

    private static Dictionary<int, List<MatchOffer>> GetMatchOffersDict(IEnumerable<MatchOffer> matchOffers)
        => matchOffers.GroupBy(a => a.OWPMatchId).ToDictionary(a => a.Key, a => a.ToList());

    private static (Dictionary<int, List<Assignment>>, Dictionary<int, List<Assignment>>) GetAssignmentsInDict(IEnumerable<Assignment> assignments)
    {
        var assignmentList = assignments.ToList();
        var assignmentsDict = assignmentList
            .GroupBy(a => a.OWPMatchId)
            .ToDictionary(a => a.Key, a => a.ToList());
        var minorAssignmentsDict = assignmentList
            .Where(x => x.ParentMatchId > 0)
            .GroupBy(a => a.ParentMatchId).ToDictionary(a => a.Key, a => a.ToList());

        return (assignmentsDict, minorAssignmentsDict);
    }

    private static (Dictionary<int, List<MatchSetting>>, Dictionary<int, List<MatchSetting>>) GetMatchSettingsInDict(IEnumerable<MatchSetting> matchSettings)
    {
        var matchSettingList = matchSettings.ToList();
        var matchSettingsDict = matchSettingList
            .GroupBy(a => a.OWPMatchId)
            .ToDictionary(a => a.Key, a => a.ToList());
        var minorMatchSettingsDict = matchSettingList
            .Where(x => x.ParentMatchId > 0)
            .GroupBy(a => a.ParentMatchId).ToDictionary(a => a.Key, a => a.ToList());

        return (matchSettingsDict, minorMatchSettingsDict);
    }

    private bool IsShowFullEventInformation() => permissionService.HasRight(FunctionKeys.FullTimeView, Constant.Policy.Read);
}