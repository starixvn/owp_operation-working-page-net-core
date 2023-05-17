using OWP.Enums;
using OWP.Extensions;
using OWP.LiveGameList.Enum;
using OWP.LiveGameList.Models;

namespace OWP.LiveGameList.Builder;

public static class AssignmentBuilder
{
    public static AssignmentModel Build(MatchInfo matchInfo, IList<Assignment> assignments)
    {
        var owpMatchId = matchInfo.OWPMatchId;
        var assignmentsDict = assignments.GroupBy(a => a.OWPMatchId).ToDictionary(a => a.Key, a => a.ToList());
        var assignmentsOfMatch = assignmentsDict.GetValue(owpMatchId, new List<Assignment>());

        var parentAssignmentsDict = assignments
            .Where(x => x.ParentMatchId > 0)
            .GroupBy(a => a.ParentMatchId).ToDictionary(a => a.Key, a => a.ToList());
        var generatedAssignments = parentAssignmentsDict.GetValue(matchInfo.OWPMatchId, new List<Assignment>());

        if (!assignmentsOfMatch.Any() && !generatedAssignments.Any())
        {
            return new AssignmentModel();
        }

        return new AssignmentModel
        {
            Handicap = GetMainAssignmentByBetType(assignmentsOfMatch, (int)MajorBetType.Handicap),
            HandicapOthers = GetOtherAssignmentByBetType(assignmentsOfMatch, (int)MajorBetType.Handicap),
            OverUnder = GetMainAssignmentByBetType(assignmentsOfMatch, (int)MajorBetType.OverUnder),
            OverUnderOthers = GetOtherAssignmentByBetType(assignmentsOfMatch, (int)MajorBetType.OverUnder),
            OneXTwo = GetMainAssignmentByBetType(assignmentsOfMatch, (int)MajorBetType.OneXTwo),
            OneXTwoOthers = GetOtherAssignmentByBetType(assignmentsOfMatch, (int)MajorBetType.OneXTwo),
            Handicap1st = GetMainAssignmentByBetType(assignmentsOfMatch, (int)MajorBetType.Handicap1St),
            Handicap1stOthers = GetOtherAssignmentByBetType(assignmentsOfMatch, (int)MajorBetType.Handicap1St),
            OverUnder1st = GetMainAssignmentByBetType(assignmentsOfMatch, (int)MajorBetType.OverUnder1St),
            OverUnder1stOthers = GetOtherAssignmentByBetType(assignmentsOfMatch, (int)MajorBetType.OverUnder1St),
            OneXTwo1st = GetMainAssignmentByBetType(assignmentsOfMatch, (int)MajorBetType.OneXTwo1St),
            OneXTwo1stOthers = GetOtherAssignmentByBetType(assignmentsOfMatch, (int)MajorBetType.OneXTwo1St),
            LA = GetMainAssignmentByBetType(assignmentsOfMatch, (int)MajorBetType.La2Nd),
            LAOthers = GetOtherAssignmentByBetType(assignmentsOfMatch, (int)MajorBetType.La2Nd),

            Specific15Mins = GetAssignmentForSpecialBetType(generatedAssignments, matchInfo, (int)MinorBetType.Specific15Mins, (int)AssignmentType.Main),
            Specific15MinsOthers = GetAssignmentForSpecialBetType(generatedAssignments, matchInfo, (int)MinorBetType.Specific15Mins, (int)AssignmentType.Others),
            Specific10Mins = GetAssignmentForSpecialBetType(generatedAssignments, matchInfo, (int)MinorBetType.Specific10Mins, (int)AssignmentType.Main),
            Specific10MinsOthers = GetAssignmentForSpecialBetType(generatedAssignments, matchInfo, (int)MinorBetType.Specific10Mins, (int)AssignmentType.Others),
            FastMarket = GetAssignmentForSpecialBetType(generatedAssignments, matchInfo, (int)MinorBetType.FastMarket, (int)AssignmentType.Main),
            FastMarketOthers = GetAssignmentForSpecialBetType(generatedAssignments, matchInfo, (int)MinorBetType.FastMarket, (int)AssignmentType.Others),
            AllInOne = GetAssignmentForSpecialBetType(generatedAssignments, matchInfo, (int)MinorBetType.AllInOne, (int)AssignmentType.Main),
            AllInOneOthers = GetAssignmentForSpecialBetType(generatedAssignments, matchInfo, (int)MinorBetType.AllInOne, (int)AssignmentType.Others)
        };
    }

    private static IEnumerable<Assignment> GetMainAssignmentByBetType(IEnumerable<Assignment> assignments, int betType)
        => assignments.Where(x => x.BettypeId == betType && x.AssignmentType == (int)AssignmentType.Main);

    private static IEnumerable<Assignment> GetOtherAssignmentByBetType(IEnumerable<Assignment> assignments, int betType)
        => assignments.Where(x => x.BettypeId == betType && x.AssignmentType == (int)AssignmentType.Others);

    private static IEnumerable<Assignment> GetAssignmentForSpecialBetType(IEnumerable<Assignment> generatedAssignments, MatchInfo matchInfo, int betTypeId, int assignmentType)
        => !matchInfo.IsMinorMatch
            ? generatedAssignments.Where(x => x.MinorBettypeId == betTypeId &&
                                                       x.BettypeId == (int)MajorBetType.Handicap &&
                                                       x.AssignmentType == assignmentType)
            : GetAssignmentForSpecific15Mins(generatedAssignments, matchInfo, betTypeId, assignmentType);

    private static IEnumerable<Assignment> GetAssignmentForSpecific15Mins(
        IEnumerable<Assignment> generatedAssignments,
        MatchInfo matchInfo,
        int betTypeId,
        int assignmentType)
        => (matchInfo.MinorBettypeId, betTypeId) switch
        {
            ((int)MinorBetType.NoOfCorner, (int)MinorBetType.Specific15Mins) => GetAssignmentForSpecific15MinsCorners(generatedAssignments, matchInfo, assignmentType),
            ((int)MinorBetType.TotalBooking, (int)MinorBetType.Specific15Mins) => GetAssignmentForSpecific15MinsBookings(generatedAssignments, matchInfo, assignmentType),
            _ => new List<Assignment>()
        };

    private static IEnumerable<Assignment> GetAssignmentForSpecific15MinsCorners(IEnumerable<Assignment> parentAssignments, MatchInfo matchInfo, int assignmentType)
        => parentAssignments.Where(x => x.MinorBettypeId == (int)MinorBetType.Specific15MinsCorners
                                  && x.BettypeId == (int)MajorBetType.Handicap
                                  && x.ParentMatchId == matchInfo.ParentMatchId
                                  && x.AssignmentType == assignmentType).ToList();

    private static IEnumerable<Assignment> GetAssignmentForSpecific15MinsBookings(IEnumerable<Assignment> assignments, MatchInfo matchInfo, int assignmentType)
        => assignments.Where(x => x.MinorBettypeId == (int)MinorBetType.Specific15MinsTotalBookings
                                  && x.BettypeId == (int)MajorBetType.Handicap
                                  && x.ParentMatchId == matchInfo.ParentMatchId
                                  && x.AssignmentType == assignmentType).ToList();
}
