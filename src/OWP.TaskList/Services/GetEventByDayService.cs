using OWP.Constants;
using OWP.TaskList.Criteria;
using Fanex.NAP.Client.Helpers.Web;
using Fanex.NAP.Core.Business;
using OWP.TaskList.Models;
using OWP.Services;
using OWP.Enums;

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

        public GetEventByDayServiceImp(Repository repository, INapSession session, PermissionService permissionService, GetAdminSiteMatchIdService getAdminSiteMatchIdService = null)
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
                UserId = 360418,
                EventDate = eventDate
            };

            var model = new TaskListModel
            {
                HasReadNotePermission = permissionService.HasRight(FunctionKeys.Note, Constant.Policy.Read),
                Matches = await GetMatchListToday(criteria),
                HasDetailInformationPermission = permissionService.HasRight(FunctionKeys.DetailsInformation, Constant.Policy.Read),
                IsHideCompletedMatches = isHideCompletedMatches
            };
            return model;
        }
        public async Task<IEnumerable<MatchInfo>> GetMatchListToday(GetTaskListCriteria criteria)
        {
            var matchInfos = new List<MatchInfo>();

            if (criteria != null && criteria.IsValid())
            {
                matchInfos = GetEvents(criteria).ToList();
            }
            BuildIPTVCopy(matchInfos);
            await BuildAdminSiteId(matchInfos);

            return matchInfos;
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
            var (matchesRaw, assignments, matchSetting, televisions, matchOffers, mainMatchInfo) = repository
                .GetTaskList(
                    criteria);

            return GetInfo(matchesRaw, assignments, matchSetting, televisions, matchOffers,
                mainMatchInfo);
        }
        private MatchSettingModel GetMatchSetting(MatchInfo matchInfo, IEnumerable<MatchSetting> matchSettings)
        {
            var matchSetting = new MatchSettingModel
            {
                Handicap = GetMatchSetting(matchSettings, matchInfo, (int)MajorBettype.Handicap),
                OverUnder = GetMatchSetting(matchSettings, matchInfo, (int)MajorBettype.OverUnder),
                OneXTwo = GetMatchSetting(matchSettings, matchInfo, (int)MajorBettype.OneXTwo),
                Handicap1st = GetMatchSetting(matchSettings, matchInfo, (int)MajorBettype.Handicap1St),
                OverUnder1st = GetMatchSetting(matchSettings, matchInfo, (int)MajorBettype.OverUnder1St),
                OneXTwo1st = GetMatchSetting(matchSettings, matchInfo, (int)MajorBettype.OneXTwo1St),
                Specific15Mins = GetMatchSettingForSpecialBettype(matchSettings, matchInfo, (int)MinorBettype.Specific15Mins),
                Specific10Mins = GetMatchSettingForSpecialBettype(matchSettings, matchInfo, (int)MinorBettype.Specific10Mins),
                FastMarket = GetMatchSettingForSpecialBettype(matchSettings, matchInfo, (int)MinorBettype.FastMarket)
            };
            return matchSetting;
        }
        private static MatchSetting GetMatchSettingForSpecialBettype(
            IEnumerable<MatchSetting> matchSettings,
            MatchInfo matchInfo,
            int bettypeId)
        {
            var result = !matchInfo.IsMinorMatch
                ? matchSettings.FirstOrDefault(x => x.MinorBettypeId == bettypeId && x.ParentMatchId == matchInfo.OWPMatchId)
                : GetMatchSettingForSpecific15Mins(matchSettings, matchInfo, bettypeId);

            return result ?? new MatchSetting();
        }
        private static MatchSetting GetMatchSettingForSpecific15Mins(
            IEnumerable<MatchSetting> matchSettings,
            MatchInfo matchInfo,
            int bettypeId)
        {
            MatchSetting result;

            if (matchInfo.MinorBettypeId == (int)MinorBettype.NoOfCorner && bettypeId == (int)MinorBettype.Specific15Mins)
            {
                result = matchSettings.FirstOrDefault(x => x.MinorBettypeId == (int)MinorBettype.Specific15MinsCorners
                            && x.BettypeId == (int)MajorBettype.Handicap
                            && x.ParentMatchId == matchInfo.ParentMatchId);
            }
            else if (matchInfo.MinorBettypeId == (int)MinorBettype.TotalBooking && bettypeId == (int)MinorBettype.Specific15Mins)
            {
                result = matchSettings.FirstOrDefault(x => x.MinorBettypeId == (int)MinorBettype.Specific15MinsTotalBookings
                            && x.BettypeId == (int)MajorBettype.Handicap
                            && x.ParentMatchId == matchInfo.ParentMatchId);
            }
            else
            {
                result = new MatchSetting();
            }

            return result ?? new MatchSetting();
        }
        private static MatchSetting GetMatchSetting(
            IEnumerable<MatchSetting> matchSettings,
            MatchInfo matchInfo,
            int bettypeId)
        {
            var result = matchSettings.FirstOrDefault(x => x.OWPMatchId == matchInfo.OWPMatchId && x.BettypeId == bettypeId);

            return result ?? new MatchSetting();
        }
        private IEnumerable<MatchInfo> GetInfo(
        IEnumerable<MatchInfo> matchInfos,
           IEnumerable<Assignment> assignments,
           IEnumerable<MatchSetting> matchSettings,
           IEnumerable<Television> televisions,
           IEnumerable<MatchOffer> matchOffers,
           IEnumerable<MainMatchInfo> mainMatchInfos)
        {
            foreach (var matchInfo in matchInfos)
            {
                matchInfo.Assignment = GetAssignment(matchInfo, assignments);
                matchInfo.MatchSetting = GetMatchSetting(matchInfo, matchSettings);
                matchInfo.Televisions = televisions.Where(x => x.OWPMatchId == matchInfo.OWPMatchId);
                matchInfo.MatchOffers = matchOffers.Where(x => x.OWPMatchId == matchInfo.OWPMatchId);

                matchInfo.Phone = matchInfo.Assignment.LA.Any(x => x.IsCurrentUser && x.BettypeId < 0 && matchInfo.IsOneHourToKickOffTime)
                    ? matchInfo.Phone : string.Empty;

                matchInfo.IsCompletedMatch = GetMainMatchCompleteStatus(matchInfo, mainMatchInfos);
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
        private AssignmentModel GetAssignment(MatchInfo matchInfo, IEnumerable<Assignment> assignments)
        {
            var assignment = new AssignmentModel
            {
                Handicap = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBettype.Handicap, (int)AssignmentType.Main),
                HandicapOthers = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBettype.Handicap, (int)AssignmentType.Others),
                OverUnder = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBettype.OverUnder, (int)AssignmentType.Main),
                OverUnderOthers = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBettype.OverUnder, (int)AssignmentType.Others),
                OneXTwo = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBettype.OneXTwo, (int)AssignmentType.Main),
                OneXTwoOthers = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBettype.OneXTwo, (int)AssignmentType.Others),
                Handicap1st = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBettype.Handicap1St, (int)AssignmentType.Main),
                Handicap1stOthers = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBettype.Handicap1St, (int)AssignmentType.Others),
                OverUnder1st = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBettype.OverUnder1St, (int)AssignmentType.Main),
                OverUnder1stOthers = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBettype.OverUnder1St, (int)AssignmentType.Others),
                OneXTwo1st = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBettype.OneXTwo1St, (int)AssignmentType.Main),
                OneXTwo1stOthers = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBettype.OneXTwo1St, (int)AssignmentType.Others),
                Specific15Mins = GetAssignmentForSpecialBettype(assignments, matchInfo, (int)MinorBettype.Specific15Mins, (int)AssignmentType.Main),
                Specific15MinsOthers = GetAssignmentForSpecialBettype(assignments, matchInfo, (int)MinorBettype.Specific15Mins, (int)AssignmentType.Others),
                Specific10Mins = GetAssignmentForSpecialBettype(assignments, matchInfo, (int)MinorBettype.Specific10Mins, (int)AssignmentType.Main),
                Specific10MinsOthers = GetAssignmentForSpecialBettype(assignments, matchInfo, (int)MinorBettype.Specific10Mins, (int)AssignmentType.Others),
                FastMarket = GetAssignmentForSpecialBettype(assignments, matchInfo, (int)MinorBettype.FastMarket, (int)AssignmentType.Main),
                FastMarketOthers = GetAssignmentForSpecialBettype(assignments, matchInfo, (int)MinorBettype.FastMarket, (int)AssignmentType.Others),
                AllInOne = GetAssignmentForSpecialBettype(assignments, matchInfo, (int)MinorBettype.AllInOne, (int)AssignmentType.Main),
                AllInOneOthers = GetAssignmentForSpecialBettype(assignments, matchInfo, (int)MinorBettype.AllInOne, (int)AssignmentType.Others),
                LA = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBettype.La2Nd, (int)AssignmentType.Main),
                LAOthers = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBettype.La2Nd, (int)AssignmentType.Others)
            };

            return assignment;
        }
        private static IEnumerable<Assignment> GetAssignmentByBettypeAndAssignmentType(IEnumerable<Assignment> assignments, MatchInfo matchInfo, int betType, int assignmentType)
        {
            return assignments.Where(x => x.OWPMatchId == matchInfo.OWPMatchId && x.BettypeId == betType && x.AssignmentType == assignmentType);
        }
        private static IEnumerable<Assignment> GetAssignmentForSpecialBettype(
            IEnumerable<Assignment> assignments,
            MatchInfo matchInfo,
            int bettypeId,
            int assignmentType)
        {
            var result = !matchInfo.IsMinorMatch ? assignments.Where(x => x.MinorBettypeId == bettypeId
                                                                                  && x.BettypeId == (int)MajorBettype.Handicap
                                                                                  && x.ParentMatchId == matchInfo.OWPMatchId
                                                                                  && x.AssignmentType == assignmentType)
                : GetAssignmentForSpecific15Mins(assignments, matchInfo, bettypeId, assignmentType);
            return result;
        }
        private static IEnumerable<Assignment> GetAssignmentForSpecific15Mins(
            IEnumerable<Assignment> assignments,
            MatchInfo matchInfo,
            int bettypeId,
            int assignmentType)
        {
            IEnumerable<Assignment> result;

            if (matchInfo.MinorBettypeId == (int)MinorBettype.NoOfCorner && bettypeId == (int)MinorBettype.Specific15Mins)
            {
                result = assignments.Where(x => x.MinorBettypeId == (int)MinorBettype.Specific15MinsCorners
                            && x.BettypeId == (int)MajorBettype.Handicap
                            && x.ParentMatchId == matchInfo.ParentMatchId
                            && x.AssignmentType == assignmentType);
            }
            else if (matchInfo.MinorBettypeId == (int)MinorBettype.TotalBooking && bettypeId == (int)MinorBettype.Specific15Mins)
            {
                result = assignments.Where(x => x.MinorBettypeId == (int)MinorBettype.Specific15MinsTotalBookings
                           && x.BettypeId == (int)MajorBettype.Handicap
                           && x.ParentMatchId == matchInfo.ParentMatchId
                           && x.AssignmentType == assignmentType);
            }
            else
            {
                result = new List<Assignment>();
            }

            return result;
        }
    }
}
