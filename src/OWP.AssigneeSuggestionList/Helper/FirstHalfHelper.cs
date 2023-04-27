using OWP.AssigneeSuggestionList.Models;
using OWP.Enums;

namespace OWP.AssigneeSuggestionList.Helper;
public static class FirstHalfHelper
{
    public static bool IsFirstHalf(IList<AssignmentModel> assignments)
        => assignments.All(
            a => a.BettypeId is (int)EnumBettype.FirstHalfHandicap or (int)EnumBettype.FirstHalfOverUnder);
}
