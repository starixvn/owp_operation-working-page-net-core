namespace OWP.TaskList.Services;

using Builders;
using Criteria;
using Fanex.NAP.Client.Helpers.Web;
using Models;
using OWP.Services;

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

    public GetEventByDayServiceImp(Repository repository, INapSession session, PermissionService permissionService, GetAdminSiteMatchIdService getAdminSiteMatchIdService)
    {
        this.repository = repository;
        this.session = session;
        this.permissionService = permissionService;
        this.getAdminSiteMatchIdService = getAdminSiteMatchIdService;
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
        await BuildAdminSiteId(matchInfo);
        return matchInfo;
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
                TeleVision = string.Join("/", tvCode),
                IpTv = match.IPTV,
                Channel = match.Channel,
                Stream = match.WatchBet
            });
        }
    }
    public IEnumerable<MatchInfo> GetEvents(GetTaskListCriteria criteria)
    {
        var (matchesRaw, assignments, matchSetting, televisions, matchOffers, mainMatchInfo, tickets) = repository
            .GetTaskList(
                criteria);

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
