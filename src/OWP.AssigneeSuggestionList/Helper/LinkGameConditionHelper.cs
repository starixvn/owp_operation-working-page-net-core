using OWP.AssigneeSuggestionList.Models;
using OWP.Enums;

namespace OWP.AssigneeSuggestionList.Helper;

public static class LinkGameConditionHelper
{
    public static bool CanGroupNGame(AssignmentModel assignment) =>
        assignment.IsMainMatch
        && !assignment.IsMajorLeague
        && assignment.MatchType == Constant.MatchType.Other
        && IsValidBetType(assignment.BettypeId);

    public static IDictionary<DateTime, List<AssignmentModel>> GroupGame(
        IList<AssignmentModel> assignments,
        Func<int, bool> numberGameInGroupFunc)
        => assignments.Where(CanGroupNGame)
            .GroupBy(a => a.EventDateTime)
            .Where(g => numberGameInGroupFunc.Invoke(g.Select(x => x.OWPMatchId).Distinct().Count()))
            .ToDictionary(x => x.Key, x => x.ToList());

    public static IDictionary<DateTime, List<AssignmentModel>> GroupGameByTimeIntervals(
        IList<AssignmentModel> assignments)
    {
        var sortedAssignments = assignments
            .Where(CanGroupNGame)
            .OrderBy(a => a.EventDateTime).ToList();

        var groupedAssignments = new Dictionary<DateTime, List<AssignmentModel>>();
        List<AssignmentModel> currentGroup = null;
        var currentGroupStartTime = sortedAssignments.FirstOrDefault(new AssignmentModel()).EventDateTime;

        foreach (var assignment in sortedAssignments)
        {
            if (currentGroup == null || (assignment.EventDateTime - currentGroupStartTime).TotalMinutes > 10)
            {
                currentGroupStartTime = assignment.EventDateTime;
                currentGroup = [assignment];
                groupedAssignments.Add(currentGroupStartTime, currentGroup);
                continue;
            }
            currentGroup.Add(assignment);
        }

        return groupedAssignments;
    }

    private static bool IsValidBetType(int bettypeId)
        => bettypeId is (int)EnumBettype.Handicap or (int)EnumBettype.OverUnder;


}