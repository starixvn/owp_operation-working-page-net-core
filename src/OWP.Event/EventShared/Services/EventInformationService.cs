using OWP.Event.EventShared.Builder;
using OWP.Event.EventShared.Models;
using OWP.Extensions;
using OWP.Models;
using System.Net;

namespace OWP.Event.EventShared.Services;

public interface EventInformationService
{
    Task GetInfositeAndAdminsiteInfo(List<MatchInfo> matchInfos);

    MatchInfo BuildEvent(EventInformation eventInfo, MatchInfo matchInfo);
}

public class EventInformationServiceImp : EventInformationService
{
    private readonly AdminSiteMatchServices adminSiteMatchServices;
    private readonly InfoSiteTimeServices infoSiteTimeService;

    private IEnumerable<MinorMatchInfo> minorAdminSiteInfos;
    private List<InfoSiteTimeModel> infositeInfos;
    private List<AdminSiteMatchInfoModel> adminsiteInfos;

    public EventInformationServiceImp(AdminSiteMatchServices adminSiteMatchServices, InfoSiteTimeServices infoSiteTimeService)
    {
        this.adminSiteMatchServices = adminSiteMatchServices;
        this.infoSiteTimeService = infoSiteTimeService;
    }

    public async Task GetInfositeAndAdminsiteInfo(List<MatchInfo> matchInfos)
    {
        minorAdminSiteInfos = await adminSiteMatchServices.GetMinorAdminSiteIds(matchInfos);

        var parentMatchIds = string.Join(',', matchInfos.Select(x => x.ParentMatchId).Distinct());
        infositeInfos = await infoSiteTimeService.GetInfositeInfo(parentMatchIds);
        adminsiteInfos = await adminSiteMatchServices.GetAdminSiteInfo(parentMatchIds);
    }

    public MatchInfo BuildEvent(EventInformation eventInfo, MatchInfo matchInfo)
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

        return matchInfo;
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
}