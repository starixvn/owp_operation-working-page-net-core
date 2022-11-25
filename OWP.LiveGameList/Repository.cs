using Fanex.Data;
using Olympus.OWP.Constants;
using OWP.Enums;
using OWP.LiveGameList.Criteria;
using OWP.LiveGameList.Enum;
using OWP.LiveGameList.Models;
using System.Net;

namespace OWP.LiveGameList
{
    public interface Repository
    {
        IEnumerable<MatchInfo> GetEventInformation(GetEventsCriteria criteria);

        Task<IEnumerable<TicketInfo>> GetTicketsByOwpMatchId(GetTicketListCriteria criteria);
    }

    public class RepositoryImp : Repository
    {

        public IEnumerable<MatchInfo> GetEventInformation(GetEventsCriteria criteria)
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

                    return GetInfo(matchInfos, assignments, matchSettings, matchNotes, televisions, matchOffers);
                }
            }
        }


        public Task<IEnumerable<TicketInfo>> GetTicketsByOwpMatchId(GetTicketListCriteria criteria)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<MatchInfo> GetInfo(
            IEnumerable<MatchInfo> matchInfos,
            IEnumerable<Assignment> assignments,
            IEnumerable<MatchSetting> matchSettings,
            IEnumerable<MatchNote> matchNotes,
            IEnumerable<Television> televisions,
            IEnumerable<MatchOffer> matchOffers)
        {
            foreach (var matchInfo in matchInfos)
            {
                matchInfo.Assignment = GetAssignment(matchInfo, assignments);
                matchInfo.MatchSetting = GetMatchSetting(matchInfo, matchSettings);
                matchInfo.MatchNotes = matchNotes.Where(x => x.OWPMatchId == matchInfo.OWPMatchId);
                matchInfo.Televisions = televisions.Where(x => x.OWPMatchId == matchInfo.OWPMatchId);
                matchInfo.MatchOffers = matchOffers.Where(x => x.OWPMatchId == matchInfo.OWPMatchId).Select(x => new MatchOffer
                {
                    OWPMatchId = x.OWPMatchId,
                    MatchOfferValue = WebUtility.HtmlEncode(x.MatchOfferValue)
                });
                matchInfo.TotalNotes = matchInfo.MatchNotes.Count();
            }

            return matchInfos;
        }

        private AssignmentModel GetAssignment(MatchInfo matchInfo, IEnumerable<Assignment> assignments)
        {
            var assignment = new AssignmentModel
            {
                Handicap = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBetTypeEnum.Handicap, (int)AssignmentType.Main),
                HandicapOthers = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBetTypeEnum.Handicap, (int)AssignmentType.Others),
                OverUnder = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBetTypeEnum.OverUnder, (int)AssignmentType.Main),
                OverUnderOthers = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBetTypeEnum.OverUnder, (int)AssignmentType.Others),
                OneXTwo = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBetTypeEnum.OneXTwo, (int)AssignmentType.Main),
                OneXTwoOthers = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBetTypeEnum.OneXTwo, (int)AssignmentType.Others),
                Handicap1st = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBetTypeEnum.Handicap1St, (int)AssignmentType.Main),
                Handicap1stOthers = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBetTypeEnum.Handicap1St, (int)AssignmentType.Others),
                OverUnder1st = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBetTypeEnum.OverUnder1St, (int)AssignmentType.Main),
                OverUnder1stOthers = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBetTypeEnum.OverUnder1St, (int)AssignmentType.Others),
                OneXTwo1st = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBetTypeEnum.OneXTwo1St, (int)AssignmentType.Main),
                OneXTwo1stOthers = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBetTypeEnum.OneXTwo1St, (int)AssignmentType.Others),
                Specific15Mins = GetAssignmentForSpecialBettype(assignments, matchInfo, (int)MinorBetTypeEnum.Specific15Mins, (int)AssignmentType.Main),
                Specific15MinsOthers = GetAssignmentForSpecialBettype(assignments, matchInfo, (int)MinorBetTypeEnum.Specific15Mins, (int)AssignmentType.Others),
                Specific10Mins = GetAssignmentForSpecialBettype(assignments, matchInfo, (int)MinorBetTypeEnum.Specific10Mins, (int)AssignmentType.Main),
                Specific10MinsOthers = GetAssignmentForSpecialBettype(assignments, matchInfo, (int)MinorBetTypeEnum.Specific10Mins, (int)AssignmentType.Others),
                FastMarket = GetAssignmentForSpecialBettype(assignments, matchInfo, (int)MinorBetTypeEnum.FastMarket, (int)AssignmentType.Main),
                FastMarketOthers = GetAssignmentForSpecialBettype(assignments, matchInfo, (int)MinorBetTypeEnum.FastMarket, (int)AssignmentType.Others),
                AllInOne = GetAssignmentForSpecialBettype(assignments, matchInfo, (int)MinorBetTypeEnum.AllInOne, (int)AssignmentType.Main),
                AllInOneOthers = GetAssignmentForSpecialBettype(assignments, matchInfo, (int)MinorBetTypeEnum.AllInOne, (int)AssignmentType.Others),
                LA = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBetTypeEnum.La2Nd, (int)AssignmentType.Main),
                LAOthers = GetAssignmentByBettypeAndAssignmentType(assignments, matchInfo, (int)MajorBetTypeEnum.La2Nd, (int)AssignmentType.Others)
            };

            return assignment;
        }

        private static MatchSetting GetMatchSetting(
            IEnumerable<MatchSetting> matchSettings,
            MatchInfo matchInfo,
            int bettypeId)
        {
            var result = matchSettings.FirstOrDefault(x => x.OWPMatchId == matchInfo.OWPMatchId && x.BettypeId == bettypeId);

            return result ?? new MatchSetting();
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
                                                                          && x.BettypeId == (int)MajorBetTypeEnum.Handicap
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

            if (matchInfo.MinorBettypeId == (int)MinorBetTypeEnum.NoOfCorner && bettypeId == (int)MinorBetTypeEnum.Specific15Mins)
            {
                result = assignments.Where(x => x.MinorBettypeId == (int)MinorBetTypeEnum.Specific15MinsCorners
                                                && x.BettypeId == (int)MajorBetTypeEnum.Handicap
                                                && x.ParentMatchId == matchInfo.ParentMatchId
                                                && x.AssignmentType == assignmentType);
            }
            else if (matchInfo.MinorBettypeId == (int)MinorBettype.TotalBooking && bettypeId == (int)MinorBetTypeEnum.Specific15Mins)
            {
                result = assignments.Where(x => x.MinorBettypeId == (int)MinorBetTypeEnum.Specific15MinsTotalBookings
                                                && x.BettypeId == (int)MajorBetTypeEnum.Handicap
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
