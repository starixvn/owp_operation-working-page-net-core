using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;
using OWP.Extensions;
using System.Collections.Immutable;
using System.Globalization;
using MatchType = OWP.AssigneeSuggestionList.Constant.MatchType;

namespace OWP.AssigneeSuggestionList.Helper;

public static class ScheduleHelper
{
    public static string BuildScheduleTextSabaGroupGame(IList<AssignmentModel> assignments)
    {
        var eventDateTime = assignments.First().EventDateTime;
        var labelList = assignments.GroupBy(g => g.MatchType)
	        .Select(g => g.Count() == 1
		        ? $"{MatchTypeLabel.MatchTypeMapLabel[(int)g.Key]}"
		        : $"{MatchTypeLabel.MatchTypeMapLabel[(int)g.Key]}*{g.Count()}");

		return eventDateTime.Minute == 0
		?$"*V/{string.Join("+",labelList)}"
		: $"*{eventDateTime:hh\\:mm}/{string.Join("+", labelList)}";
    }

    public static string BuildScheduleTextGroupGame(IList<AssignmentModel> assignments)
    {
	    var assignmentsToDistinct = assignments
		    .DistinctBy(x => x.MaxbetHDP)
		    .ToList();

	    return assignmentsToDistinct.Count() == 1
		    ? BuildTextGameGroupSameMb(assignmentsToDistinct.First())
		    : BuildTextGameGroupMixedMb(assignmentsToDistinct);
    }

    private static string BuildTextGameGroupMixedMb(IList<AssignmentModel> assignments)
    {
	    var assignment = assignments.First();
		var listMaxbet = assignments
			.Where(a => a.MaxbetHDP.HasValue)
			.Select(a => $"{FormatMaxbet(a.MaxbetHDP.Value)}");

		var maxbetListText = string.Join("+", listMaxbet);
		var eventDateTime = assignment.EventDateTime;
		var kOTimeText = $"{eventDateTime:hh\\:mm}";

		return (eventDateTime.Minute == 0, string.IsNullOrEmpty(maxbetListText)) switch
		{
			(true, true) => "V",
			(true, false) => $"V/{maxbetListText}",
			(false, true) => $"{kOTimeText}",
			(false, false) => $"{kOTimeText}/{maxbetListText}"
		};
    }

	private static string BuildTextGameGroupSameMb(AssignmentModel assignment)
    {
	    var eventDateTime = assignment.EventDateTime;
	    var maxbet = assignment.MaxbetHDP;

	    var kOTimeText = $"{eventDateTime:hh\\:mm}";
		return (eventDateTime.Minute == 0, maxbet.HasValue) switch
	    {
		    (true, false) => "V",
		    (true, true) => $"V/{FormatMaxbet(maxbet.Value)}",
		    (false, true) => $"{kOTimeText}/{FormatMaxbet(maxbet.Value)}",
		    (false, false) => $"{kOTimeText}"
	    };
	}

	public static string BuildScheduleTextSabaSingleGame(AssignmentModel assignment)
    => assignment.KOTime.Minutes == 0
        ? "*V"
        : $"*{assignment.KOTime.ToKOTimeWithoutSuffix()}";

    public static string BuildScheduleTextSingleGame(AssignmentModel assignment)
    {
        var maxbetDisplay = assignment.MaxbetHDP.HasValue
                ? $"{FormatMaxbet(assignment.MaxbetHDP.Value)}"
            : string.Empty;
        var kOTimeText = assignment.KOTime.ToKOTimeWithoutSuffix();
        return (assignment.KOTime.Minutes == 0, string.IsNullOrEmpty(maxbetDisplay)) switch
        {
            (true, true) => "V",
            (false, true) => $"{kOTimeText}",
            (true, false) => $"V/{maxbetDisplay}",
            (false, false) => $"{kOTimeText}/{maxbetDisplay}"
        };
    }

    public static (DateTime eventDate, IList<int> bettypeIds, int? minorBettypeId, string remark, decimal? maxbetHdp) GetGameInfo(IList<AssignmentModel> assignments)
    {
        var firstAssignment = assignments.First();
        var bettypeIds = assignments.Select(x => x.BettypeId).Distinct().ToList();

        return (
            firstAssignment.EventDateTime,
            bettypeIds,
            firstAssignment.MinorBettypeId,
            firstAssignment.Remark,
            firstAssignment.MaxbetHDP);
    }

    public static string FormatMaxbet(decimal maxbet)
        => $"{(maxbet / 1000).ToString("0.####", CultureInfo.InvariantCulture)}K";

    public static (string, string) GetBackgroundAndTextColor(MatchType matchType)
        => ColorCodeConst.ColorByGroupName.GetValueOrDefault(matchType, ColorCodeConst.ErrorColor);
}