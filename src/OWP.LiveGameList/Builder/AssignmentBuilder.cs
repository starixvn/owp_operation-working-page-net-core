namespace OWP.LiveGameList.Builder;

using Enum;
using Enums;
using Models;

public static class AssignmentBuilder
{
    public static AssignmentModel Build(MatchInfo matchInfo, IList<Assignment> assignments)
        => new()
        {
            Handicap = GetAssignmentByBetTypeAndAssignmentType(assignments, matchInfo, (int)MajorBetType.Handicap, (int)AssignmentType.Main),
            HandicapOthers = GetAssignmentByBetTypeAndAssignmentType(assignments, matchInfo, (int)MajorBetType.Handicap, (int)AssignmentType.Others),
            OverUnder = GetAssignmentByBetTypeAndAssignmentType(assignments, matchInfo, (int)MajorBetType.OverUnder, (int)AssignmentType.Main),
            OverUnderOthers = GetAssignmentByBetTypeAndAssignmentType(assignments, matchInfo, (int)MajorBetType.OverUnder, (int)AssignmentType.Others),
            OneXTwo = GetAssignmentByBetTypeAndAssignmentType(assignments, matchInfo, (int)MajorBetType.OneXTwo, (int)AssignmentType.Main),
            OneXTwoOthers = GetAssignmentByBetTypeAndAssignmentType(assignments, matchInfo, (int)MajorBetType.OneXTwo, (int)AssignmentType.Others),
            Handicap1st = GetAssignmentByBetTypeAndAssignmentType(assignments, matchInfo, (int)MajorBetType.Handicap1St, (int)AssignmentType.Main),
            Handicap1stOthers = GetAssignmentByBetTypeAndAssignmentType(assignments, matchInfo, (int)MajorBetType.Handicap1St, (int)AssignmentType.Others),
            OverUnder1st = GetAssignmentByBetTypeAndAssignmentType(assignments, matchInfo, (int)MajorBetType.OverUnder1St, (int)AssignmentType.Main),
            OverUnder1stOthers = GetAssignmentByBetTypeAndAssignmentType(assignments, matchInfo, (int)MajorBetType.OverUnder1St, (int)AssignmentType.Others),
            OneXTwo1st = GetAssignmentByBetTypeAndAssignmentType(assignments, matchInfo, (int)MajorBetType.OneXTwo1St, (int)AssignmentType.Main),
            OneXTwo1stOthers = GetAssignmentByBetTypeAndAssignmentType(assignments, matchInfo, (int)MajorBetType.OneXTwo1St, (int)AssignmentType.Others),
            Specific15Mins = GetAssignmentForSpecialBetType(assignments, matchInfo, (int)MinorBetType.Specific15Mins, (int)AssignmentType.Main),
            Specific15MinsOthers = GetAssignmentForSpecialBetType(assignments, matchInfo, (int)MinorBetType.Specific15Mins, (int)AssignmentType.Others),
            Specific10Mins = GetAssignmentForSpecialBetType(assignments, matchInfo, (int)MinorBetType.Specific10Mins, (int)AssignmentType.Main),
            Specific10MinsOthers = GetAssignmentForSpecialBetType(assignments, matchInfo, (int)MinorBetType.Specific10Mins, (int)AssignmentType.Others),
            FastMarket = GetAssignmentForSpecialBetType(assignments, matchInfo, (int)MinorBetType.FastMarket, (int)AssignmentType.Main),
            FastMarketOthers = GetAssignmentForSpecialBetType(assignments, matchInfo, (int)MinorBetType.FastMarket, (int)AssignmentType.Others),
            AllInOne = GetAssignmentForSpecialBetType(assignments, matchInfo, (int)MinorBetType.AllInOne, (int)AssignmentType.Main),
            AllInOneOthers = GetAssignmentForSpecialBetType(assignments, matchInfo, (int)MinorBetType.AllInOne, (int)AssignmentType.Others),
            LA = GetAssignmentByBetTypeAndAssignmentType(assignments, matchInfo, (int)MajorBetType.La2Nd, (int)AssignmentType.Main),
            LAOthers = GetAssignmentByBetTypeAndAssignmentType(assignments, matchInfo, (int)MajorBetType.La2Nd, (int)AssignmentType.Others)
        };


    private static IEnumerable<Assignment> GetAssignmentByBetTypeAndAssignmentType(IEnumerable<Assignment> assignments, MatchInfo matchInfo, int betType, int assignmentType)
       => assignments.Where(x => x.OWPMatchId == matchInfo.OWPMatchId && x.BettypeId == betType && x.AssignmentType == assignmentType);

    private static IEnumerable<Assignment> GetAssignmentForSpecialBetType(IEnumerable<Assignment> assignments, MatchInfo matchInfo, int bettypeId, int assignmentType)
        => !matchInfo.IsMinorMatch ? assignments.Where(x => x.MinorBettypeId == bettypeId
                                                                            && x.BettypeId == (int)MajorBetType.Handicap
                                                                            && x.ParentMatchId == matchInfo.OWPMatchId
                                                                            && x.AssignmentType == assignmentType)
            : GetAssignmentForSpecific15Mins(assignments, matchInfo, bettypeId, assignmentType);

    private static IEnumerable<Assignment> GetAssignmentForSpecific15Mins(
        IEnumerable<Assignment> assignments,
        MatchInfo matchInfo,
        int bettypeId,
        int assignmentType)
    {
        if (matchInfo.MinorBettypeId == (int)MinorBetType.NoOfCorner && bettypeId == (int)MinorBetType.Specific15Mins)
        {
            return GetAssignmentForSpecific15MinsCorners(assignments, matchInfo, assignmentType);
        }
        if (matchInfo.MinorBettypeId == (int)MinorBetType.TotalBooking && bettypeId == (int)MinorBetType.Specific15Mins)

        {
            return GetAssignmentForSpecific15MinsBookings(assignments, matchInfo, assignmentType);
        }

        return new List<Assignment>();
    }

    private static IEnumerable<Assignment> GetAssignmentForSpecific15MinsCorners(IEnumerable<Assignment> assignments,
        MatchInfo matchInfo, int assignmentType)
        => assignments.Where(x => x.MinorBettypeId == (int)MinorBetType.Specific15MinsCorners
                                  && x.BettypeId == (int)MajorBetType.Handicap
                                  && x.ParentMatchId == matchInfo.ParentMatchId
                                  && x.AssignmentType == assignmentType).ToList();

    private static IEnumerable<Assignment> GetAssignmentForSpecific15MinsBookings(IEnumerable<Assignment> assignments,
        MatchInfo matchInfo, int assignmentType)
        => assignments.Where(x => x.MinorBettypeId == (int)MinorBetType.Specific15MinsTotalBookings
                                  && x.BettypeId == (int)MajorBetType.Handicap
                                  && x.ParentMatchId == matchInfo.ParentMatchId
                                  && x.AssignmentType == assignmentType).ToList();
}



