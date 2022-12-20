using Fanex.NAP.Client.Helpers.Web;
using Fanex.NAP.Core.Business;
using OWP.Constants;
using OWP.Services;
using OWP.TaskList.Builders;
using OWP.TaskList.Criteria;
using OWP.TaskList.Models;

namespace OWP.TaskList.Services
{
    public interface GetEventByDayService
    {
        Task<TaskListModel> GetEventByDay(DateTime eventDate, bool isHideCompletedMatches);
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

        public async Task<TaskListModel> GetEventByDay(DateTime eventDate, bool isHideCompletedMatches)
        {
            var criteria = new GetTaskListCriteria
            {
                UserId = 8,
                EventDate = eventDate
            };

            var model = new TaskListModel
            {
                Matches = await GetMatchListToday(criteria),
                HasDetailInformationPermission = true,
                IsHideCompletedMatches = isHideCompletedMatches
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
            FakeData(matchInfo);
            return matchInfo;
        }

        private void FakeData(IList<MatchInfo> matchInfo)
        {
            int i = 1;
            matchInfo.Add(matchInfo.First().Clone());
            matchInfo.Add(matchInfo.First().Clone());
            matchInfo.Add(matchInfo.First().Clone());
            matchInfo.Add(matchInfo.First().Clone());
            matchInfo.Add(matchInfo.First().Clone());
            matchInfo.Add(matchInfo.First().Clone());

            foreach (var item in matchInfo)
            {
                item.MatchNo = i.ToString();
                item.Provider = "provider";
                item.WatchBet = "WatchBet";
                item.Phone = "Phone";
                item.OtherProvider = "OtherProvider";
                item.AdminSiteId = i;
                item.Remark = "Remark";
                item.Round = "Round";
                item.Division = "Division";
                item.Televisions = new List<Television>(){
                    new Television{
                        TVCode= "TV code",
                        Color="red"
                    }
                };
                item.Channel = "channel";
                if (i == 1)
                {
                    item.League = item.League + "FIFA SABA ";
                    item.MatchStatus = "Live";
                    item.OtherTime = new TimeSpan(5, 6, 22);
                    item.NoOfHDP = "NoOfHDP";
                    item.NoOfHDPId = 1;
                    item.MatchCode = "MatchCode";
                    item.MatchOffers = new List<MatchOffer>{
                        new MatchOffer{
                            MatchOfferValue= "MatchOfferValue",
                        },
                        new MatchOffer{
                            MatchOfferValue= "MatchOfferValue1",
                        }
                    };
                }
                if (i == 2)
                {
                    item.League = item.League + " PES ";
                    item.MatchStatus = "In-play";
                    item.OtherTime = new TimeSpan(5, 6, 22);
                    item.NoOfHDP = "NoOfHDP";
                    item.NoOfHDPId = 2;
                    item.MatchCode = "MatchCode";
                    item.MatchOffers = new List<MatchOffer>{
                        new MatchOffer{
                            MatchOfferValue= "MatchOfferValue",
                        },
                        new MatchOffer{
                            MatchOfferValue= "MatchOfferValue1",
                        }
                    };
                }
                if (i == 3)
                {
                    item.MatchStatus = "Completed";

                    item.OtherTime = new TimeSpan(5, 6, 22);
                }
                if (i == 4)
                {
                    item.MatchStatus = "Cancel";
                }
                if (i == 5)
                {
                    item.MatchStatus = "Postponed";
                }
                if (i == 6)
                {
                    item.MatchStatus = "Suspended";
                }
                item.NoOfHDPId = i;
                i++;
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
}
