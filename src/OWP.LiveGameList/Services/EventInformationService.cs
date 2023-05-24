using Fanex.NAP.Client.Helpers.Web;
using Fanex.NAP.Core.Business;
using OWP.Constants;
using OWP.Extensions;
using OWP.LiveGameList.Builder;
using OWP.LiveGameList.Models;
using OWP.Models;
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
    private readonly AdminSiteMatchServices adminSiteMatchServices;
    private readonly PermissionService permissionService;
    private readonly Repository repository;
    private readonly InfoSiteTimeServices infoSiteTimeService;

    public EventInformationServiceImp(
        Repository repository,
        INapSession currentSession,
        AdminSiteMatchServices adminSiteMatchServices,
        PermissionService permissionService,
        InfoSiteTimeServices infoSiteTimeService)
    {
        this.repository = repository;
        this.currentSession = currentSession;
        this.adminSiteMatchServices = adminSiteMatchServices;
        this.permissionService = permissionService;
        this.infoSiteTimeService = infoSiteTimeService;
    }

    public async Task<IEnumerable<MatchInfo>> GetEvents(DateTime eventDate, string statusId)
    {
        var eventInformation = repository.GetEventInformation(eventDate, statusId, currentSession.UserId, IsShowFullEventInformation());
        var minorAdminSiteInfos = await adminSiteMatchServices.GetMinorAdminSiteIds(eventInformation.MatchInfos);

        var parentMatchIds = string.Join(',', eventInformation.MatchInfos.Select(x => x.ParentMatchId).Distinct());
        var infositeInfos = await infoSiteTimeService.GetInfositeInfo(parentMatchIds);
        var adminsiteInfos = await adminSiteMatchServices.GetAdminSiteInfo(parentMatchIds);

        return BuildEvent(eventInformation, minorAdminSiteInfos.ToList(), infositeInfos, adminsiteInfos);
    }

    private IEnumerable<MatchInfo> BuildEvent(
        EventInformation eventInfo,
        IList<MinorMatchInfo> minorAdminSiteInfos,
        List<InfoSiteTimeModel> infositeInfos,
        List<AdminSiteMatchInfoModel> adminsiteInfos)
    {
        foreach (var matchInfo in eventInfo.MatchInfos)
        {
            var owpMatchId = matchInfo.OWPMatchId;

            matchInfo.Assignment = AssignmentBuilder.Build(matchInfo, eventInfo.AssignmentsDict, eventInfo.MinorAssignmentsDict);
            matchInfo.MatchSetting = MatchSettingBuilder.Build(matchInfo, eventInfo.MatchSettingsDict, eventInfo.MinorMatchSettingsDict);
            matchInfo.Televisions = BuildTelevisions(eventInfo, owpMatchId);
            matchInfo.MatchOffers = BuildMatchOffers(eventInfo, owpMatchId);
            matchInfo.IPTVCopy = BuildIptvCopy(matchInfo);
            matchInfo.TicketInfos = BuildTicketInfos(eventInfo, owpMatchId);

            adminSiteMatchServices.UpdateMinorAdminMatchId(matchInfo, minorAdminSiteInfos);
            infoSiteTimeService.BuildInfoSiteTimeAndAbsenceIcon(matchInfo, infositeInfos, adminsiteInfos);

            LengthPropertyBuilder.BuildLengthProperty(matchInfo);

            yield return matchInfo;
        }
    }

    private static IEnumerable<Television> BuildTelevisions(EventInformation eventInfo, int owpMatchId)
        => eventInfo.TelevisionsDict.GetValue(owpMatchId, new List<Television>());

    private static IEnumerable<TicketInfo> BuildTicketInfos(EventInformation eventInfo, int owpMatchId)
        => eventInfo.TicketInfosDict.GetValue(owpMatchId, new List<TicketInfo>())
            .OrderByDescending(x => x.LatestCreatedDate);

    private static string BuildIptvCopy(MatchInfo matchInfo)
        => IPTVBuilder.Build(new IPTVModel
        {
            Television = string.Join("/", matchInfo.Televisions.Select(t => t.TVCode)),
            IpTv = matchInfo.IPTV,
            Channel = matchInfo.Channel,
            Stream = matchInfo.WatchBet
        });

    private static IEnumerable<MatchOffer> BuildMatchOffers(EventInformation eventInfo, int owpMatchId)
        => eventInfo.MatchOffersDict.GetValue(owpMatchId, new List<MatchOffer>())
            .Select(x => new MatchOffer
            {
                OWPMatchId = x.OWPMatchId,
                MatchOfferValue = WebUtility.HtmlEncode(x.MatchOfferValue)
            });

    private bool IsShowFullEventInformation() => permissionService.HasRight(FunctionKeys.FullTimeView, Constant.Policy.Read);
}