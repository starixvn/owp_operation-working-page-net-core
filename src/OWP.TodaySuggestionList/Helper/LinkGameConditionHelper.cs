using OWP.Enums;

namespace OWP.TodaySuggestionList.Helper
{
    public class LinkGameConditionHelper
    {
        public static bool CanGroupNGame(AssignmentModel a) =>
                a.IsMainMatch
                && !a.IsMajorLeague
                && a.BettypeId==(int) EnumBettype.Handicap
                || a.BettypeId==(int) EnumBettype.OverUnder
                && maxbetModels is not null
                && maxbetModels.Any(m => m.OWPMatchId==a.OWPMatchId
                                            && m.BettypeId== a.BettypeId
                                            && m.Maxbet <= MAX_MAXBET);
    }
}