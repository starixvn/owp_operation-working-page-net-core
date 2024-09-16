using OWP.AssigneeSuggestionList.Constant;

namespace OWP.AssigneeSuggestionList.Helper;

using Extensions;
using Models;
using MatchType = Constant.MatchType;

public static class CategorizeHelper
{
    public static bool HasWorkingShiftInRangeTime(this UserInfo userInfo, DateTime kOTime, DateTime endTime)
        => userInfo.ShiftStartTime <= kOTime && userInfo.ShiftEndTime >= endTime;

    public static bool IsFreeInRangeTime(this List<AssignmentModel> assignments, DateTime kOTime, DateTime endTime)
        => !assignments.Exists(x => x.EventDateTime < endTime && kOTime < x.EndTime);

    public static List<AssignmentModel> GetSameKOTimeAssignments(this IEnumerable<AssignmentModel> assignments, MatchInfo matchInfo)
        => assignments.Where(x =>
                x.IsSaba == matchInfo.IsSaba &&
                x.EventDateTime.CompareTo(matchInfo.KOTime) == 0 &&
                (x.IsSaba || !x.IsSingleMatch)).ToList();

    public static bool ExistsValue(this List<string> mainBetTypeLst, string value)
        => mainBetTypeLst.Exists(x => x.Trim().Equals(value, StringComparison.OrdinalIgnoreCase));

    public static string FormatExplanation(string text, MatchInfo matchInfo, bool isPartially = false)
    {
        const string timeFormat = "hh:mm tt";
        var toTime = isPartially ? matchInfo.KOTime.AddHours(1) : matchInfo.EndTime;
        return string.Format(text, matchInfo.KOTime.ToString(timeFormat), toTime.ToString(timeFormat));
    }

    public static MatchType GetSabaBetType(this string leagueName) // NOSONAR
        => leagueName switch
        {
            _ when leagueName.IsContains("M88 SABA") && leagueName.IsContains("- 16 Mins Play") => MatchType.SabaM8816Mins,
            _ when leagueName.IsContains("- 16 Mins Play") => MatchType.Saba16Mins,
            _ when leagueName.IsContains("- 6 Mins Play") => MatchType.SabaFutsal6Mins,
            _ when leagueName.IsContains("- 15 Mins Play") => MatchType.Saba15Mins,
            _ when leagueName.IsContains("PES ") && leagueName.IsContains("- 5 Mins Play") => MatchType.Saba5Mins,
            _ when leagueName.IsContains("- 20 Mins Play") => MatchType.Saba20Mins,
            _ when leagueName.IsContains("- PENALTY SHOOTOUTS") => MatchType.SabaShootOuts,
            _ => MatchType.SabaOther
        };

    public static bool IsMultipleType(this MatchType matchType)
        => CategoryConstants.MultipleGameTypes.Contains(matchType);
}
