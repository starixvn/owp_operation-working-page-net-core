using OWP.AssigneeSuggestionList.Models;
using OWP.Enums;

namespace OWP.AssigneeSuggestionList.Helper
{
    public class LinkGameConditionHelper
    {
        public static bool CanGroupNGame(AssignmentModel assignment, IList<MaxbetModel> maxbetModels, Func<decimal, bool> maxbetComparerFunc) =>
                assignment.IsMainMatch
                && !assignment.IsMajorLeague
                && (Equals(assignment.BettypeId, EnumBettype.Handicap)
                || Equals(assignment.BettypeId, EnumBettype.OverUnder))
                && maxbetModels is not null
                && maxbetModels.Any(m => Equals(m.OWPMatchId, assignment.OWPMatchId)
                                            && Equals(m.BettypeId, assignment.BettypeId)
                                            && maxbetComparerFunc.Invoke(m.Maxbet));
    }
}