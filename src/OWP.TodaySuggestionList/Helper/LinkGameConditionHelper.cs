using OWP.Enums;

namespace OWP.TodaySuggestionList.Helper
{
    public class LinkGameConditionHelper
    {
        public static bool CanGroupNGame(AssignmentModel a) =>
                a.IsMainMatch
                && !a.IsMajorLeague
                && (Equals(a.BettypeId, EnumBettype.Handicap)
                || Equals(a.BettypeId, EnumBettype.OverUnder))
                && maxbetModels is not null
                && maxbetModels.Any(m => Equals(m.OWPMatchId, a.OWPMatchId)
                                            && Equals(m.BettypeId, a.BettypeId)
                                            && m.Maxbet <= MAX_MAXBET);
    }
}