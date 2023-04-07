using OWP.AssigneeSuggestionList.Models;
using OWP.Enums;

namespace OWP.AssigneeSuggestionList.Helper;

public static class LinkGameConditionHelper
{
    public static bool CanGroupNGame(AssignmentModel assignment, Func<decimal?, bool> maxbetComparerFunc) =>
        assignment.IsMainMatch
        && !assignment.IsMajorLeague
        && IsValidBetType(assignment.BettypeId)
        && maxbetComparerFunc.Invoke(assignment.MaxbetHDP);

    private static bool IsValidBetType(int bettypeId)
        => bettypeId == (int)EnumBettype.Handicap
           || bettypeId == (int)EnumBettype.OverUnder;
}