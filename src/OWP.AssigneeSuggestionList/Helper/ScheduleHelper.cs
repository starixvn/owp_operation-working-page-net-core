using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;
using System.Collections.Immutable;
using System.Globalization;
using MatchType = OWP.AssigneeSuggestionList.Constant.MatchType;

namespace OWP.AssigneeSuggestionList.Helper;

public static class ScheduleHelper
{
    public static string BuildScheduleTextGroupGame(IList<AssignmentModel> assignments)
    {
        var eventDateTime = assignments.First().EventDateTime;
        var listMaxbet = new List<string>();
        foreach (var assignment in assignments)
        {
            if (assignment.MaxbetHDP.HasValue)
            {
                listMaxbet.Add($"{FormatMaxbet(assignment.MaxbetHDP.Value)}");
            }
        }

        var kOTimeText = $"{eventDateTime:hh\\:mm}";
        var maxbetListText = string.Join("+", listMaxbet);

        string text;
        if (string.IsNullOrWhiteSpace(maxbetListText))
        {
            text = eventDateTime.Minute == 0
            ? $"V"
            : $"{kOTimeText}";
        }
        else
        {
            text = eventDateTime.Minute == 0
            ? $"V/{maxbetListText}"
            : $"{kOTimeText}/{maxbetListText}";
        }
        return assignments.First().IsSaba ? $"*{text}" : text;
    }

    public static string BuildScheduleTextSingleGame(AssignmentModel assignment)
    {
        var maxbetDisplay = assignment.MaxbetHDP.HasValue
                ? $"{FormatMaxbet(assignment.MaxbetHDP.Value)}"
            : string.Empty;
        var kOTimeText = $"{assignment.KOTime:hh\\:mm}";
        string text;
        if (string.IsNullOrWhiteSpace(maxbetDisplay))
        {
            text = assignment.KOTime.Minutes == 0
            ? $"V"
            : $"{kOTimeText}";
        }
        else
        {
            text = assignment.KOTime.Minutes == 0
                        ? $"V/{maxbetDisplay}"
                        : $"{kOTimeText}/{maxbetDisplay}";
        }
        return assignment.IsSaba ? $"*{text}" : text;
    }

    public static (DateTime eventDate, int owpMatchId, IList<int> bettypeIds, string remark) GetGameInfo(IList<AssignmentModel> assignments)
    {
        var gameInfo = assignments.First();
        var eventDate = gameInfo.EventDateTime;
        var owpMatchId = gameInfo.OWPMatchId;
        var Remark = gameInfo.Remark;

        var bettypeIds = assignments.Select(x => x.BettypeId).Distinct().ToList();
        return (eventDate, owpMatchId, bettypeIds, Remark);
    }

    public static (DateTime eventDate, int owpMatchId, IList<int> bettypeIds, int? parentMatchId, string remark) GetGameInfoMinor(IList<AssignmentModel> assignments)
    {
        var parentMatchId = assignments.First().ParentMatchId;
        var gameInfo = GetGameInfo(assignments);
        return (
            gameInfo.eventDate,
            gameInfo.owpMatchId,
            gameInfo.bettypeIds,
            parentMatchId,
            gameInfo.remark);
    }

    public static string FormatMaxbet(decimal maxbet)
        => $"{(maxbet / 1000).ToString("0.####", CultureInfo.InvariantCulture)}K";

    public static (string, string) GetBackgroundAndTextColor(MatchType matchType)
        => ColorCodeConst.ColorByGroupName.GetValueOrDefault(matchType, ColorCodeConst.ErrorColor);
}