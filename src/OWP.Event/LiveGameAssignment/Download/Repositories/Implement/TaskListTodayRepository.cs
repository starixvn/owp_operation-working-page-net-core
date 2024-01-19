namespace Fanex.NAP.OWP.TaskScheduling.Repositories.Implement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Data.Repository;
    using global::OWP.Enumerations;
    using Interfaces;
    using Model;
    using Models.TaskListToday;
    using Assignment = Models.TaskListToday.Assignment;
    using MatchInfo = Models.TaskListToday.MatchInfo;
    using MatchSetting = Models.TaskListToday.MatchSetting;

    public class TaskListTodayRepository : ITaskListTodayRepository
    {
        public IEnumerable<MatchInfo> GetMatchListToday(ICriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.QueryMultiple(criteria))
                {
                    var matchInfos = reader.Read<MatchInfo>();
                    var assignments = reader.Read<Assignment>();
                    var matchSettings = reader.Read<MatchSetting>();
                    var matchNotes = reader.Read<MatchNote>();
                    var televisions = reader.Read<Television>();
                    var matchOffers = reader.Read<MatchOffer>();
                    var mainMatchInfo = reader.Read<MainMatchInfo>();

                    return GetInfo(matchInfos, assignments, matchSettings, matchNotes, televisions, matchOffers, mainMatchInfo);
                }
            }
        }

        public IEnumerable<DateTime> UpdateTradeStatus(ICriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.QueryMultiple(criteria))
                {
                    return reader.Read<DateTime>();
                }
            }
        }

        private static IEnumerable<Assignment> GetAssignmentForSpecialBettype(
            IEnumerable<Assignment> assignments,
            MatchInfo matchInfo,
            int bettypeId,
            int assignmentType)
        {
            var result = !matchInfo.IsMinorMatch ? assignments.Where(x => x.MinorBettypeId == bettypeId
                                                                                  && x.BettypeId == (int)MajorBetType.Handicap
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

            if (matchInfo.MinorBettypeId == (int)MinorBetType.NoOfCorner && bettypeId == (int)MinorBetType.Specific15Mins)
            {
                result = assignments.Where(x => x.MinorBettypeId == (int)MinorBetType.Specific15MinsCorners
                            && x.BettypeId == (int)MajorBetType.Handicap
                            && x.ParentMatchId == matchInfo.ParentMatchId
                            && x.AssignmentType == assignmentType);
            }
            else if (matchInfo.MinorBettypeId == (int)MinorBetType.TotalBooking && bettypeId == (int)MinorBetType.Specific15Mins)
            {
                result = assignments.Where(x => x.MinorBettypeId == (int)MinorBetType.Specific15MinsTotalBookings
                           && x.BettypeId == (int)MajorBetType.Handicap
                           && x.ParentMatchId == matchInfo.ParentMatchId
                           && x.AssignmentType == assignmentType);
            }
            else
            {
                result = new List<Assignment>();
            }

            return result;
        }

        private AssignmentModel GetAssignment(MatchInfo matchInfo, IEnumerable<Assignment> assignments)
        {
            var assignment = new AssignmentModel
            {
                Handicap = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBetType.Handicap, (int)AssignmentType.Main),
                HandicapOthers = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBetType.Handicap, (int)AssignmentType.Others),
                OverUnder = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBetType.OverUnder, (int)AssignmentType.Main),
                OverUnderOthers = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBetType.OverUnder, (int)AssignmentType.Others),
                OneXTwo = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBetType.OneXTwo, (int)AssignmentType.Main),
                OneXTwoOthers = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBetType.OneXTwo, (int)AssignmentType.Others),
                Handicap1st = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBetType.Handicap1St, (int)AssignmentType.Main),
                Handicap1stOthers = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBetType.Handicap1St, (int)AssignmentType.Others),
                OverUnder1st = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBetType.OverUnder1St, (int)AssignmentType.Main),
                OverUnder1stOthers = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBetType.OverUnder1St, (int)AssignmentType.Others),
                OneXTwo1st = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBetType.OneXTwo1St, (int)AssignmentType.Main),
                OneXTwo1stOthers = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBetType.OneXTwo1St, (int)AssignmentType.Others),
                Specific15Mins = GetAssignmentForSpecialBettype(assignments, matchInfo, (int)MinorBetType.Specific15Mins, (int)AssignmentType.Main),
                Specific15MinsOthers = GetAssignmentForSpecialBettype(assignments, matchInfo, (int)MinorBetType.Specific15Mins, (int)AssignmentType.Others),
                Specific10Mins = GetAssignmentForSpecialBettype(assignments, matchInfo, (int)MinorBetType.Specific10Mins, (int)AssignmentType.Main),
                Specific10MinsOthers = GetAssignmentForSpecialBettype(assignments, matchInfo, (int)MinorBetType.Specific10Mins, (int)AssignmentType.Others),
                FastMarket = GetAssignmentForSpecialBettype(assignments, matchInfo, (int)MinorBetType.FastMarket, (int)AssignmentType.Main),
                FastMarketOthers = GetAssignmentForSpecialBettype(assignments, matchInfo, (int)MinorBetType.FastMarket, (int)AssignmentType.Others),
                AllInOne = GetAssignmentForSpecialBettype(assignments, matchInfo, (int)MinorBetType.AllInOne, (int)AssignmentType.Main),
                AllInOneOthers = GetAssignmentForSpecialBettype(assignments, matchInfo, (int)MinorBetType.AllInOne, (int)AssignmentType.Others),
                LA = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBetType.La2Nd, (int)AssignmentType.Main),
                LAOthers = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBetType.La2Nd, (int)AssignmentType.Others)
            };

            return assignment;
        }

        private static IEnumerable<Assignment> GetAssignmentByBettypeAndAssignmentType(IEnumerable<Assignment> assignments, MatchInfo matchInfo, int betType, int assignmentType)
        {
            return assignments.Where(x => x.OWPMatchId == matchInfo.OWPMatchId && x.BettypeId == betType && x.AssignmentType == assignmentType);
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

            if (matchInfo.MinorBettypeId == (int)MinorBetType.NoOfCorner && bettypeId == (int)MinorBetType.Specific15Mins)
            {
                result = matchSettings.FirstOrDefault(x => x.MinorBettypeId == (int)MinorBetType.Specific15MinsCorners
                            && x.BettypeId == (int)MajorBetType.Handicap
                            && x.ParentMatchId == matchInfo.ParentMatchId);
            }
            else if (matchInfo.MinorBettypeId == (int)MinorBetType.TotalBooking && bettypeId == (int)MinorBetType.Specific15Mins)
            {
                result = matchSettings.FirstOrDefault(x => x.MinorBettypeId == (int)MinorBetType.Specific15MinsTotalBookings
                            && x.BettypeId == (int)MajorBetType.Handicap
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

        private MatchSettingModel GetMatchSetting(MatchInfo matchInfo, IEnumerable<MatchSetting> matchSettings)
        {
            var matchSetting = new MatchSettingModel
            {
                Handicap = GetMatchSetting(matchSettings, matchInfo, (int)MajorBetType.Handicap),
                OverUnder = GetMatchSetting(matchSettings, matchInfo, (int)MajorBetType.OverUnder),
                OneXTwo = GetMatchSetting(matchSettings, matchInfo, (int)MajorBetType.OneXTwo),
                Handicap1st = GetMatchSetting(matchSettings, matchInfo, (int)MajorBetType.Handicap1St),
                OverUnder1st = GetMatchSetting(matchSettings, matchInfo, (int)MajorBetType.OverUnder1St),
                OneXTwo1st = GetMatchSetting(matchSettings, matchInfo, (int)MajorBetType.OneXTwo1St),
                Specific15Mins = GetMatchSettingForSpecialBettype(matchSettings, matchInfo, (int)MinorBetType.Specific15Mins),
                Specific10Mins = GetMatchSettingForSpecialBettype(matchSettings, matchInfo, (int)MinorBetType.Specific10Mins),
                FastMarket = GetMatchSettingForSpecialBettype(matchSettings, matchInfo, (int)MinorBetType.FastMarket)
            };
            return matchSetting;
        }

        private IEnumerable<MatchInfo> GetInfo(
           IEnumerable<MatchInfo> matchInfos,
           IEnumerable<Assignment> assignments,
           IEnumerable<MatchSetting> matchSettings,
           IEnumerable<MatchNote> matchNotes,
           IEnumerable<Television> televisions,
           IEnumerable<MatchOffer> matchOffers,
           IEnumerable<MainMatchInfo> mainMatchInfos)
        {
            foreach (var matchInfo in matchInfos)
            {
                matchInfo.Assignment = GetAssignment(matchInfo, assignments);
                matchInfo.MatchSetting = GetMatchSetting(matchInfo, matchSettings);
                matchInfo.MatchNotes = matchNotes.Where(x => x.OWPMatchId == matchInfo.OWPMatchId);
                matchInfo.Televisions = televisions.Where(x => x.OWPMatchId == matchInfo.OWPMatchId);
                matchInfo.MatchOffers = matchOffers.Where(x => x.OWPMatchId == matchInfo.OWPMatchId);

                matchInfo.Phone = matchInfo.Assignment.LA.Any(x => x.IsCurrentUser && x.BettypeId < 0 && matchInfo.IsOneHourToKickOffTime)
                    ? matchInfo.Phone : string.Empty;

                matchInfo.TotalNotes = matchInfo.MatchNotes.Count();
                matchInfo.IsHasNewNote = matchInfo.MatchNotes.Any(x => !x.IsViewed);
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
    }
}