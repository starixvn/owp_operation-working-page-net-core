
using OWP.TaskList.Enumerations;

namespace OWP.TaskList.Builders;

using Enums;
using Models;

public static class AssignmentBuilder
{
    public static AssignmentModel Build(MatchInfo matchInfo, IEnumerable<Assignment> assignments)
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
            result = GetAssignmentForSpecific15MinsCorners(assignments, matchInfo, assignmentType);
        }
        else if (matchInfo.MinorBettypeId == (int)MinorBettype.TotalBooking && bettypeId == (int)MinorBettype.Specific15Mins)
        {
            result = GetAssignmentForSpecific15MinsBookings(assignments, matchInfo, assignmentType);
        }
        else
        {
            result = new List<Assignment>();
        }

        return result;
    }

    private static IEnumerable<Assignment> GetAssignmentForSpecific15MinsCorners(IEnumerable<Assignment> assignments,
        MatchInfo matchInfo, int assignmentType)
        => assignments.Where(x => x.MinorBettypeId == (int)MinorBettype.Specific15MinsCorners
                                  && x.BettypeId == (int)MajorBettype.Handicap
                                  && x.ParentMatchId == matchInfo.ParentMatchId
                                  && x.AssignmentType == assignmentType).ToList();

    private static IEnumerable<Assignment> GetAssignmentForSpecific15MinsBookings(IEnumerable<Assignment> assignments,
        MatchInfo matchInfo, int assignmentType)
        => assignments.Where(x => x.MinorBettypeId == (int)MinorBettype.Specific15MinsTotalBookings
                                  && x.BettypeId == (int)MajorBettype.Handicap
                                  && x.ParentMatchId == matchInfo.ParentMatchId
                                  && x.AssignmentType == assignmentType).ToList();

}