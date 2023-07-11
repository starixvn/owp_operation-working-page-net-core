using OWP.AssigneeSuggestionList.Models;
using OWP.Enums;

namespace OWP.AssigneeSuggestionList.Helper;

public static class LinkGameConditionHelper
{
    public static bool CanGroupNGame(AssignmentModel assignment) =>
        assignment.IsMainMatch
        && !assignment.IsMajorLeague
        && IsValidBetType(assignment.BettypeId);

    public static IDictionary<DateTime, List<AssignmentModel>> GroupGame(
        IList<AssignmentModel> assignments,
        Func<int, bool> numberGameInGroupFunc)
        => assignments.Where(CanGroupNGame)
            .GroupBy(a => a.EventDateTime)
            .Where(g => numberGameInGroupFunc.Invoke(g.Select(x => x.OWPMatchId).Distinct().Count()))
            .ToDictionary(x => x.Key, x => x.ToList());
    private static bool IsValidBetType(int bettypeId)
        => bettypeId is (int)EnumBettype.Handicap or (int)EnumBettype.OverUnder;


}