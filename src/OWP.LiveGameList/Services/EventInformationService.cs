using Fanex.NAP.Client.Helpers.Web;
using Fanex.NAP.Core.Business;
using OWP.Constants;
using OWP.LiveGameList.Builder;
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
    private readonly Repository repository;
    private readonly INapSession currentSession;
    private readonly PermissionService permissionService;
    private readonly GetAdminSiteMatchIdService getAdminSiteMatchIdService;


    public EventInformationServiceImp(Repository repository, INapSession currentSession, GetAdminSiteMatchIdService getAdminSiteMatchIdService, PermissionService permissionService)
    {
        this.repository = repository;
        this.currentSession = currentSession;
        this.getAdminSiteMatchIdService = getAdminSiteMatchIdService;
        this.permissionService = permissionService;
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

        var match = matchesRaw.ToList();
        BuildEvent(match, assignments.ToList(), matchSetting.ToList(), televisions.ToList(), matchOffers.ToList(), tickets.ToList());
        await getAdminSiteMatchIdService.UpdateMinorAdminMatchId(match);
        return match;
    }

    private void BuildEvent(
        IList<MatchInfo> matchInfos,
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
                TeleVision = string.Join("/", matchInfo.Televisions.Select(t => t.TVCode)),
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

            matchInfo.LengthLeague = matchInfo.League.Length +
                                     matchInfo.MatchOffers.Select(x => x.MatchOfferValue.Length).Sum();
            matchInfo.LengthRemark = matchInfo.Remark.Length + matchInfo.Round.Length + matchInfo.Division.Length;

            matchInfo.LengthFtTraderHdp = matchInfo.Assignment.Handicap.Select(x => x.Username.Length * 4).Sum() +
                                          matchInfo.Assignment.HandicapOthers.Select(x => x.AssignmentNote.Length * 4)
                                              .Sum();

            matchInfo.LengthFtTraderOu = matchInfo.Assignment.OverUnder.Select(x => x.Username.Length * 4).Sum() +
                                         matchInfo.Assignment.OverUnderOthers.Select(x => x.AssignmentNote.Length * 4)
                                             .Sum();

            matchInfo.Length1HTraderHdp = matchInfo.Assignment.Handicap1st.Select(x => x.Username.Length * 4).Sum() +
                                          matchInfo.Assignment.Handicap1stOthers.Select(x => x.AssignmentNote.Length * 4)
                                              .Sum();
            matchInfo.Length1HTraderOu = matchInfo.Assignment.OverUnder1st.Select(x => x.Username.Length * 4).Sum() +
                                         matchInfo.Assignment.OverUnder1stOthers.Select(x => x.AssignmentNote.Length * 4)
                                             .Sum();

            matchInfo.LengthLa = matchInfo.Assignment.LA.Select(x => x.Username.Length * 4).Sum() +
                                 matchInfo.Assignment.LAOthers.Select(x => x.AssignmentNote.Length * 4).Sum();

            matchInfo.LengthIpTv = matchInfo.Televisions.Select(x => x.TVCode.Length * 4).Sum();

            matchInfo.LengthProvider = matchInfo.Provider.Length + matchInfo.WatchBet.Length + matchInfo.Phone.Length;



        }
    }

    private bool IsShowFullEventInformation()
        => permissionService.HasRight(FunctionKeys.FullTimeView, Constant.Policy.Read);

}

