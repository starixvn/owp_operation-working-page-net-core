using OWP.AssigneeSuggestionList.Models;
using OWP.Enums;

namespace OWP.AssigneeSuggestionList.Helper
{
    public class LinkGameConditionHelper
    {
        public static bool CanGroupNGame(AssignmentModel assignment, Func<decimal?, bool> maxbetComparerFunc) =>
                assignment.IsMainMatch
                && !assignment.IsMajorLeague
                && (Equals(assignment.BettypeId, EnumBettype.Handicap)
                || Equals(assignment.BettypeId, EnumBettype.OverUnder))
                && maxbetComparerFunc.Invoke(assignment.Maxbet);
    }
}