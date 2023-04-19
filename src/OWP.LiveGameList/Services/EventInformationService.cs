using System.Net;
using Fanex.NAP.Client.Helpers.Web;
using Fanex.NAP.Core.Business;
using OWP.Constants;
using OWP.LiveGameList.Builder;
using OWP.LiveGameList.Const;
using OWP.LiveGameList.Criteria;
using OWP.LiveGameList.Models;
using OWP.Models;
using OWP.Services;

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


    public EventInformationServiceImp(Repository repository, INapSession currentSession,
        GetAdminSiteMatchIdService getAdminSiteMatchIdService, PermissionService permissionService, GetInfoSiteTimeService getInfoSiteTimeService, GetAdminSiteInfoService getAdminSiteInfoService)
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
        var infoSiteInfo = await GetInfoSiteInfo();

        var adminSiteInfo = await GetAdminSiteInfo();

        foreach (var matchInfo in matchInfos)
        {
            var infoSite = infoSiteInfo.FirstOrDefault(x => x.ParentMatchId == matchInfo.ParentMatchId);
            var adminSite = adminSiteInfo.FirstOrDefault(x => x.ParentMatchId == matchInfo.ParentMatchId);
            matchInfo.InfoSiteTime = infoSite?.KickOffTime;
            matchInfo.InfoSiteTimeDisplay = infoSite?.KichOffTimeDisplay;
            matchInfo.IsShowAbsenceIcon = adminSite?.IsShowAbsenceIcon ?? false;
        }    
    }
    
    public async Task<IList<InfoSiteTimeModel>> GetInfoSiteInfo()
    {
        var parentMatchIds = string.Join(',', matchInfos.Select(x => x.ParentMatchId).Distinct());

        return (await getInfoSiteTimeService.GetInfoSiteTime(parentMatchIds)).ToList();
    }

    public async Task<IList<AdminSiteInfoModel>> GetAdminSiteInfo()
    {
        var parentMatchIds = string.Join(',', matchInfos
            .Where(y => !LiveGameListConst.StatusNotOffered.Contains(y.MatchStatus))
            .Select(x => x.ParentMatchId)
            .Distinct());

        return (await getAdminSiteInfoService.GetAdminSiteInfo(parentMatchIds)).ToList();
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
        BuildEvent(assignments.ToList(), matchSetting.ToList(), televisions.ToList(), matchOffers.ToList(),
            tickets.ToList());

        await getAdminSiteMatchIdService.UpdateMinorAdminMatchId(matchInfos);
        await BuildInfoMatch();
        return matchInfos;
    }

    private void BuildEvent(
        IList<Assignment> assignments,
        IList<MatchSetting> matchSettings,
        IList<Television> televisions,
        IList<MatchOffer> matchOffers,
        IList<TicketInfo> tickets)
    {
        foreach (var matchInfo in matchInfos)
        {
            matchInfo.Assignment = AssignmentBuilder.Build(matchInfo, assignments);
            matchInfo.MatchSetting = MatchSettingBuilder.Build(matchInfo, matchSettings);
            matchInfo.Televisions = televisions.Where(x => x.OWPMatchId == matchInfo.OWPMatchId);
            matchInfo.MatchOffers = matchOffers.Where(x => x.OWPMatchId == matchInfo.OWPMatchId)
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
            matchInfo.TicketInfos = ticketByMatchId
                .OrderByDescending(x => x.LatestCreatedDate);
            matchInfo.HasTicket = ticketByMatchId.Any();
            matchInfo.IsRead = ticketByMatchId.All(x => x.IsRead);
            LengthPropertyBuilder.BuildLengthProperty(matchInfo);
        }
    }

    private bool IsShowFullEventInformation()
    {
        return permissionService.HasRight(FunctionKeys.FullTimeView, Constant.Policy.Read);
    }
}