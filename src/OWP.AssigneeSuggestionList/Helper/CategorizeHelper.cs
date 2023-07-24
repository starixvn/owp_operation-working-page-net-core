using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Helper;

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
}
