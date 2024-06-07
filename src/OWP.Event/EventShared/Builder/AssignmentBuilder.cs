using OWP.Enums;
using OWP.Event.EventShared.Models;
using OWP.Extensions;

namespace OWP.Event.EventShared.Builder;

public static class AssignmentBuilder
{
    public static AssignmentModel Build(MatchInfo matchInfo, Dictionary<int, List<Assignment>> assignmentsDict, Dictionary<int, List<Assignment>> minorAssignmentsDict)
    {
        var assignments = assignmentsDict.GetValue(matchInfo.OWPMatchId, new List<Assignment>());
        var generatedAssignments = minorAssignmentsDict.GetValue(matchInfo.ParentMatchId, new List<Assignment>());

        if (!assignments.Any() && !generatedAssignments.Any())
        {
            return new AssignmentModel();
        }

        return new AssignmentModel
        {
            Handicap = GetMainAssignmentByBetType(assignments, (int)MajorBetType.Handicap),
            HandicapOthers = GetOtherAssignmentByBetType(assignments, (int)MajorBetType.Handicap),
            OverUnder = GetMainAssignmentByBetType(assignments, (int)MajorBetType.OverUnder),
            OverUnderOthers = GetOtherAssignmentByBetType(assignments, (int)MajorBetType.OverUnder),
            OneXTwo = GetMainAssignmentByBetType(assignments, (int)MajorBetType.OneXTwo),
            OneXTwoOthers = GetOtherAssignmentByBetType(assignments, (int)MajorBetType.OneXTwo),
            Handicap1st = GetMainAssignmentByBetType(assignments, (int)MajorBetType.Handicap1St),
            Handicap1stOthers = GetOtherAssignmentByBetType(assignments, (int)MajorBetType.Handicap1St),
            OverUnder1st = GetMainAssignmentByBetType(assignments, (int)MajorBetType.OverUnder1St),
            OverUnder1stOthers = GetOtherAssignmentByBetType(assignments, (int)MajorBetType.OverUnder1St),
            OneXTwo1st = GetMainAssignmentByBetType(assignments, (int)MajorBetType.OneXTwo1St),
            OneXTwo1stOthers = GetOtherAssignmentByBetType(assignments, (int)MajorBetType.OneXTwo1St),
            LA = GetMainAssignmentByBetType(assignments, (int)MajorBetType.La2Nd),
            LAOthers = GetOtherAssignmentByBetType(assignments, (int)MajorBetType.La2Nd),
            // Special bettype
            Specific10Mins = GetAssignmentForSpecific10MinsBetType(generatedAssignments, matchInfo, (int)MinorBetType.Specific10Mins, (int)AssignmentType.Main),
            Specific10MinsOthers = GetAssignmentForSpecific10MinsBetType(generatedAssignments, matchInfo, (int)MinorBetType.Specific10Mins, (int)AssignmentType.Others),
            FastMarket = GetAssignmentForFastMarketBetType(generatedAssignments, matchInfo, (int)MinorBetType.FastMarket, (int)AssignmentType.Main),
            FastMarketOthers = GetAssignmentForFastMarketBetType(generatedAssignments, matchInfo, (int)MinorBetType.FastMarket, (int)AssignmentType.Others),
            AllInOne = GetAssignmentForAllInOneBetType(generatedAssignments, matchInfo, (int)MinorBetType.AllInOne, (int)AssignmentType.Main),
            AllInOneOthers = GetAssignmentForAllInOneBetType(generatedAssignments, matchInfo, (int)MinorBetType.AllInOne, (int)AssignmentType.Others),
            // Specific 15 mins
            Specific15Mins = GetAssignmentForSpecific15MinsBetType(generatedAssignments, matchInfo, (int)AssignmentType.Main),
            Specific15MinsOthers = GetAssignmentForSpecific15MinsBetType(generatedAssignments, matchInfo, (int)AssignmentType.Others)
        };
    }

    private static IEnumerable<Assignment> GetMainAssignmentByBetType(IEnumerable<Assignment> assignments, int betType)
        => assignments.Where(x => x.BettypeId == betType && x.AssignmentType == (int)AssignmentType.Main);

    private static IEnumerable<Assignment> GetOtherAssignmentByBetType(IEnumerable<Assignment> assignments, int betType)
        => assignments.Where(x => x.BettypeId == betType && x.AssignmentType == (int)AssignmentType.Others);

    private static IEnumerable<Assignment> GetAssignmentForSpecific10MinsBetType(IEnumerable<Assignment> generatedAssignments, MatchInfo matchInfo, int minorBetTypeId, int assignmentType)
        => matchInfo.IsMinorMatch
            ? new List<Assignment>()
            : GetGeneratedAssignmentForMainMatch(generatedAssignments, minorBetTypeId, assignmentType);

    private static IEnumerable<Assignment> GetAssignmentForAllInOneBetType(IEnumerable<Assignment> generatedAssignments, MatchInfo matchInfo, int minorBetTypeId, int assignmentType)
        => matchInfo.IsMinorMatch
            ? GetAssignmentSpecialProduct(generatedAssignments, matchInfo, assignmentType, (int)MinorBetType.AllInOneCorners, (int)MinorBetType.AllInOneTotalBookings)
            : GetGeneratedAssignmentForMainMatch(generatedAssignments, minorBetTypeId, assignmentType);

    private static IEnumerable<Assignment> GetAssignmentForFastMarketBetType(IEnumerable<Assignment> generatedAssignments, MatchInfo matchInfo, int minorBetTypeId, int assignmentType)
        => matchInfo.IsMinorMatch
            ? GetAssignmentSpecialProduct(generatedAssignments, matchInfo, assignmentType, (int)MinorBetType.FastMarketCorners, (int)MinorBetType.FastMarketTotalBookings)
            : GetGeneratedAssignmentForMainMatch(generatedAssignments, minorBetTypeId, assignmentType);
    private static IEnumerable<Assignment> GetAssignmentForSpecific15MinsBetType(IEnumerable<Assignment> generatedAssignments, MatchInfo matchInfo, int assignmentType)
        => matchInfo.IsMinorMatch
            ? GetAssignmentSpecialProduct(generatedAssignments, matchInfo, assignmentType, (int)MinorBetType.Specific15MinsCorners, (int)MinorBetType.Specific15MinsTotalBookings)
            : GetGeneratedAssignmentForMainMatch(generatedAssignments, (int)MinorBetType.Specific15Mins, assignmentType);

    private static IEnumerable<Assignment> GetAssignmentSpecialProduct(
        IEnumerable<Assignment> generatedAssignments,
        MatchInfo matchInfo,
        int assignmentType,
        int minorBetTypeCorners,
        int minorBetTypeTotalBookings)
    {
        var refMinorBettypeId = matchInfo.MinorBettypeId switch
        {
            (int)MinorBetType.NoOfCorner => minorBetTypeCorners,
            (int)MinorBetType.TotalBooking => minorBetTypeTotalBookings,
            _ => 0
        };

        if (refMinorBettypeId == 0)
        {
            return new List<Assignment>();
        }

        return generatedAssignments.Where(x =>
            x.MinorBettypeId == refMinorBettypeId &&
            x.BettypeId == (int)MajorBetType.Handicap &&
            x.ParentMatchId == matchInfo.ParentMatchId &&
            x.AssignmentType == assignmentType);
    }


    private static IEnumerable<Assignment> GetGeneratedAssignmentForMainMatch(IEnumerable<Assignment> generatedAssignments, int minorBetTypeId, int assignmentType)
    {
        return generatedAssignments.Where(x =>
            x.MinorBettypeId == minorBetTypeId &&
            x.BettypeId == (int)MajorBetType.Handicap &&
            x.AssignmentType == assignmentType);
    }
}
