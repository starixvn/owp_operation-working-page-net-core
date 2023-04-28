using Fanex.NAP.Client.Helpers.Web;
using OWP.Helpers;
using OWP.Models;
using OWP.Services;
using OWP.TaskList.Builders;
using OWP.TaskList.Const;
using OWP.TaskList.Criteria;
using OWP.TaskList.Models;

namespace OWP.TaskList.Services;

public interface GetEventByDayService
{
    Task<TaskListModel> GetEventByDay(DateTime eventDate);
}
public class GetEventByDayServiceImp : GetEventByDayService
{
    private readonly Repository repository;

    private readonly INapSession session;

    private readonly PermissionService permissionService;

    private readonly GetAdminSiteMatchIdService getAdminSiteMatchIdService;

    private readonly GetInfoSiteTimeService getInfoSiteTimeService;

    private readonly GetAdminSiteInfoService getAdminSiteInfoService;

    public GetEventByDayServiceImp(Repository repository, INapSession session, PermissionService permissionService,
        GetAdminSiteMatchIdService getAdminSiteMatchIdService, GetInfoSiteTimeService getInfoSiteTimeService,
        GetAdminSiteInfoService getAdminSiteInfoService)
    {
        this.repository = repository;
        this.session = session;
        this.permissionService = permissionService;
        this.getAdminSiteMatchIdService = getAdminSiteMatchIdService;
        this.getInfoSiteTimeService = getInfoSiteTimeService;
        this.getAdminSiteInfoService = getAdminSiteInfoService;
    }

    public async Task<TaskListModel> GetEventByDay(DateTime eventDate)
    {
        var criteria = new GetTaskListCriteria
        {
            UserId = session.UserId,
            EventDate = eventDate
        };

        var model = new TaskListModel
        {
            Matches = await GetMatchListToday(criteria),
            HasDetailInformationPermission = true
        };
        return model;
    }
    public async Task<IEnumerable<MatchInfo>> GetMatchListToday(GetTaskListCriteria criteria)
    {
        var matchInfo = new List<MatchInfo>();

        if (criteria != null && criteria.IsValid())
        {
            matchInfo = GetEvents(criteria).ToList();
        }
        BuildIPTVCopy(matchInfo);
        await BuildInfoMatch(matchInfo);
        await BuildAdminSiteId(matchInfo);
        return matchInfo;
    }

    public async Task BuildInfoMatch(IList<MatchInfo> matches)
    {
        var parentMatchIds = string.Join(',', matches.Select(x => x.ParentMatchId).Distinct());

        var infoSiteInfo = (await getInfoSiteTimeService.GetInfoSiteTime(parentMatchIds)).ToList();

        var adminSiteIdsWithOwpMatches = (await getAdminSiteInfoService.GetAdminSiteMatchId(parentMatchIds)).ToList();

        var adminSitesIdsOdds = (await getAdminSiteInfoService.GetAdminSiteInfo()).ToList();

        var adminSiteMatchInfo = LiveGameHelper.MergeAdminSiteInfo(adminSiteIdsWithOwpMatches, adminSitesIdsOdds);

        foreach (var matchInfo in matches)
        {
            var infoSite = infoSiteInfo.FirstOrDefault(x => x.ParentMatchId == matchInfo.ParentMatchId);
            var isShowAbsenceIcon = adminSiteMatchInfo.Any(x => x.ParentMatchId == matchInfo.OWPMatchId);
            matchInfo.InfoSiteTime = infoSite?.KickOffTime;
            matchInfo.InfoSiteTimeDisplay = infoSite?.KickOffTimeDisplay;
            matchInfo.IsShowAbsenceIcon = !TaskListConst.StatusNotOffered.Contains(matchInfo.MatchStatus) && isShowAbsenceIcon;
        }
    }

    private async Task BuildAdminSiteId(IList<MatchInfo> matches)
    {
        foreach (var match in matches)
        {
            match.AdminSiteId = await getAdminSiteMatchIdService.GetAdminSiteMatchId(match.OWPMatchId,
                match.ParentMatchId, match.MinorBettypeId);

        }
    }
    private void BuildIPTVCopy(IList<MatchInfo> matches)
    {
        foreach (var match in matches)
        {
            var tvCode = match.Televisions.Select(t => t.TVCode);
            match.IPTVCopy = IPTVBuilder.Build(new IPTVModel
            {
                Television = string.Join("/", tvCode),
                IpTv = match.IPTV,
                Channel = match.Channel,
                Stream = match.WatchBet
            });
        }
    }

    public IEnumerable<MatchInfo> GetEvents(GetTaskListCriteria criteria)
    {
        var (matchesRaw, assignments, matchSetting, televisions, matchOffers, mainMatchInfo, tickets) = repository
            .GetTaskList(criteria);

        return GetInfo(matchesRaw, assignments, matchSetting, televisions, matchOffers,
            mainMatchInfo, tickets);
    }

    private IEnumerable<MatchInfo> GetInfo(
    IEnumerable<MatchInfo> matchInfos,
       IEnumerable<Assignment> assignments,
       IEnumerable<MatchSetting> matchSettings,
       IEnumerable<Television> televisions,
       IEnumerable<MatchOffer> matchOffers,
       IEnumerable<MainMatchInfo> mainMatchInfos,
       IEnumerable<TicketInfo> tickets)
    {
        foreach (var matchInfo in matchInfos)
        {
            matchInfo.Assignment = AssignmentBuilder.Build(matchInfo, assignments);
            matchInfo.MatchSetting = MatchSettingBuilder.Build(matchInfo, matchSettings);
            matchInfo.Televisions = televisions.Where(x => x.OWPMatchId == matchInfo.OWPMatchId);
            matchInfo.MatchOffers = matchOffers.Where(x => x.OWPMatchId == matchInfo.OWPMatchId);

            matchInfo.Phone = matchInfo.Assignment.LA.Any(x => x.IsCurrentUser && x.BettypeId < 0 && matchInfo.IsOneHourToKickOffTime)
                ? matchInfo.Phone : string.Empty;

            matchInfo.IsCompletedMatch = GetMainMatchCompleteStatus(matchInfo, mainMatchInfos);
            var ticketByMatchId = tickets
                .Where(x => x.OwpMatchId == matchInfo.OWPMatchId)
                .ToList();
            matchInfo.TicketInfos = ticketByMatchId
                .OrderByDescending(x => x.LatestCreatedDate);
            matchInfo.HasTicket = ticketByMatchId.Any();
            matchInfo.IsRead = ticketByMatchId.All(x => x.IsRead);

        }

        return matchInfos;
    }
    private bool GetMainMatchCompleteStatus(MatchInfo matchInfo, IEnumerable<MainMatchInfo> mainMatchInfos)
    {
        if (!matchInfo.MinorBettypeId.HasValue)
        {
            return matchInfo.IsCompletedMatch;
        }

        var mainMatch = mainMatchInfos.FirstOrDefault(x => x.OWPMatchId == matchInfo.ParentMatchId);

        return mainMatch == null ? matchInfo.IsCompletedMatch : mainMatch.IsCompletedMatch;
    }
}
