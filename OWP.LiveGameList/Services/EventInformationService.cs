using Fanex.NAP.Client.Helpers.Web;
using OWP.LiveGameList.Criteria;
using OWP.LiveGameList.Helper;
using OWP.LiveGameList.Models;

namespace OWP.LiveGameList.Services
{
    public interface EventInformationService
    {
        Task<IEnumerable<MatchInfo>> GetEvents(GetEventsCriteria criteria);
    }

    public class EventInformationServiceImp : EventInformationService
    {
        private readonly Repository repository;
        private readonly INapSession currentSession;
        private readonly GetAdminSiteMatchIdService getAdminSiteMatchIdService;

        public EventInformationServiceImp(Repository repository, INapSession currentSession, GetAdminSiteMatchIdService getAdminSiteMatchIdService)
        {
            this.repository = repository;
            this.currentSession = currentSession;
            this.getAdminSiteMatchIdService = getAdminSiteMatchIdService;
        }

        public async Task<IEnumerable<MatchInfo>> GetEvents(GetEventsCriteria criteria)
        {
            var matches = repository.GetEventInformation(criteria).ToList();
            BuildIPTVCopy(matches);
            await BuildAdminSiteId(matches);
            await BuildTicketList(matches);

            return matches;
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
                match.IPTVCopy = LiveGameListHelper.BuildIpTv(new IPTVModel
                {
                    TeleVision = string.Join("/", tvCode),
                    IpTv = match.IPTV,
                    Channel = match.Channel,
                    Stream = match.WatchBet
                });
            }
        }

        private async Task BuildTicketList(List<MatchInfo> matches)
        {
            string matchIds = string.Join(",", matches.Select(x => x.OWPMatchId));
            int userId = currentSession.UserId;
            var tickets = (await repository
                    .GetTicketsByOwpMatchId(new GetTicketListCriteria
                    { OWPMatchIds = matchIds, ActorId = userId }))
                .ToList();
            foreach (var match in matches)
            {
                match.TicketInfos = tickets.Where(x => x.OwpMatchId == match.OWPMatchId)
                    .OrderByDescending(x => x.LatestCreatedDate);
            }
        }

    }
}
