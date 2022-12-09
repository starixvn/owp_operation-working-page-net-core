namespace OWP.LiveGameList.Services;

using System.Net;
using Builder;
using Constants;
using Criteria;
using Fanex.NAP.Client.Helpers.Web;
using Fanex.NAP.Core.Business;
using Models;
using OWP.Services;

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

    private List<MatchInfo> matchInfos = new();


    public EventInformationServiceImp(Repository repository, INapSession currentSession,
        GetAdminSiteMatchIdService getAdminSiteMatchIdService, PermissionService permissionService)
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

        matchInfos = matchesRaw.ToList();
        BuildEvent(assignments.ToList(), matchSetting.ToList(), televisions.ToList(), matchOffers.ToList(),
            tickets.ToList());

        await getAdminSiteMatchIdService.UpdateMinorAdminMatchId(matchInfos);
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
            BuildLengthProperty(matchInfo);
        }
    }

    private void BuildLengthProperty(MatchInfo matchInfo)
    {
        matchInfo.LengthLeague = matchInfo.League.Length +
                                 matchInfo.MatchOffers.Select(x => x.MatchOfferValue.Length).Sum();

        matchInfo.LengthRemark = matchInfo.Remark.Length + matchInfo.Round.Length + matchInfo.Division.Length;

        matchInfo.LengthTraderOu = SumLengthTrader(matchInfo.Assignment.OverUnder,
            matchInfo.Assignment.OverUnderOthers) + SumLengthTrader(
            matchInfo.Assignment.OverUnder1st, matchInfo.Assignment.OverUnder1stOthers);

        matchInfo.LengthTraderHdp = SumLengthTrader(matchInfo.Assignment.Handicap,
            matchInfo.Assignment.HandicapOthers) + SumLengthTrader(
            matchInfo.Assignment.Handicap1st, matchInfo.Assignment.Handicap1stOthers);

        matchInfo.LengthOtherProduct = SumLengthOtherProduct(matchInfo);

        matchInfo.NumberOfOtherProduct = matchInfo.Assignment.Specific10Mins.Count() +
                                         matchInfo.Assignment.Specific15Mins.Count() +
                                         matchInfo.Assignment.AllInOne.Count() +
                                         matchInfo.Assignment.FastMarket.Count();

        matchInfo.LengthLa =
            SumLengthTrader(matchInfo.Assignment.LA, matchInfo.Assignment.LAOthers);

        matchInfo.LengthIpTv = matchInfo.Televisions.Select(x => x.TVCode.Length * 4).Sum();

        var lengthProvider = string.IsNullOrEmpty(matchInfo.Provider) ? 0 : matchInfo.Provider.Length * 4;
        matchInfo.LengthProvider = lengthProvider + matchInfo.WatchBet.Length + matchInfo.Phone.Length;
    }

    private int SumLengthOtherProduct(MatchInfo matchInfo)
    {
        return SumLengthTrader(matchInfo.Assignment.OneXTwo,
                   matchInfo.Assignment.OneXTwoOthers) +
               SumLengthTrader(matchInfo.Assignment.OneXTwo1st,
                   matchInfo.Assignment.OneXTwo1stOthers) +
               SumLengthTrader(matchInfo.Assignment.FastMarket,
                   matchInfo.Assignment.FastMarketOthers) +
               SumLengthTrader(matchInfo.Assignment.AllInOne,
                   matchInfo.Assignment.AllInOneOthers) +
               SumLengthTrader(matchInfo.Assignment.Specific15Mins,
                   matchInfo.Assignment.Specific15MinsOthers) +
               SumLengthTrader(matchInfo.Assignment.Specific10Mins,
                   matchInfo.Assignment.Specific10MinsOthers);
        ;
    }

    private int SumLengthTrader(IEnumerable<Assignment> assignments, IEnumerable<Assignment> assignmentsOther)
    {
        return (assignments.Select(x => string.IsNullOrEmpty(x.Username) ? 0 : x.Username.Length).Sum() +
                assignmentsOther.Select(x => string.IsNullOrEmpty(x.AssignmentNote) ? 0 : x.AssignmentNote.Length).Sum()) * 2;
    }

    private bool IsShowFullEventInformation()
    {
        return permissionService.HasRight(FunctionKeys.FullTimeView, Constant.Policy.Read);
    }
}