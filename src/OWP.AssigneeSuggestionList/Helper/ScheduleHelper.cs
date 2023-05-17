using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;
using OWP.Extensions;
using System.Collections.Immutable;
using System.Globalization;
using MatchType = OWP.AssigneeSuggestionList.Constant.MatchType;

namespace OWP.AssigneeSuggestionList.Helper;

public static class ScheduleHelper
{
    public static string BuildScheduleTextGroupGame(IList<AssignmentModel> assignments)
    {
        var eventDateTime = assignments.First().EventDateTime;
        var listMaxbet = assignments
            .Where(assignment => assignment.MaxbetHDP.HasValue)
            .Select(assignment => $"{FormatMaxbet(assignment.MaxbetHDP.Value)}");

        var kOTimeText = $"{eventDateTime:hh\\:mm}";
        var maxbetListText = string.Join("+", listMaxbet);

        var text = (eventDateTime.Minute == 0, string.IsNullOrEmpty(maxbetListText)) switch
        {
            (true, true) => "V",
            (true, false) => $"V/{maxbetListText}",
            (false, true) => $"{kOTimeText}",
            (false, false) => $"{kOTimeText}/{maxbetListText}"
        };

        return assignments.First().IsSaba ? $"*{text}" : text;
    }

    public static string BuildScheduleTextSingleGame(AssignmentModel assignment)
    {
        var maxbetDisplay = assignment.MaxbetHDP.HasValue
                ? $"{FormatMaxbet(assignment.MaxbetHDP.Value)}"
            : string.Empty;
        var kOTimeText = assignment.KOTime.ToKOTimeWithoutSuffix();
        var text = (assignment.KOTime.Minutes == 0, string.IsNullOrEmpty(maxbetDisplay)) switch
        {
            (true, true) => "V",
            (false, true) => $"{kOTimeText}",
            (true, false) => $"V/{maxbetDisplay}",
            (false, false) => $"{kOTimeText}/{maxbetDisplay}"
        };
        return assignment.IsSaba ? $"*{text}" : text;
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