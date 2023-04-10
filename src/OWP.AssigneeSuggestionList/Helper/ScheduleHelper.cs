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
        var kOTimeText = $"{assignment.KOTime:hh\\:mm}";
        var text = (assignment.KOTime.Minutes == 0, string.IsNullOrEmpty(maxbetDisplay)) switch
        {
            (true, true) => "V",
            (false, true) => $"{kOTimeText}",
            (true, false) => $"V/{maxbetDisplay}",
            (false, false) => $"{kOTimeText}/{maxbetDisplay}"
        };
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

    public static (DateTime eventDate, int owpMatchId, IList<int> bettypeIds, int? parentMatchId, string remark, decimal? maxbetHdp) GetGameInfoMinor(IList<AssignmentModel> assignments)
    {
        var parentMatchId = assignments.First().ParentMatchId;
        var maxbetHdp = assignments.First().MaxbetHDP;
        var gameInfo = GetGameInfo(assignments);
        return (
            gameInfo.eventDate,
            gameInfo.owpMatchId,
            gameInfo.bettypeIds,
            parentMatchId,
            gameInfo.remark,
            maxbetHdp);
    }

    public static string FormatMaxbet(decimal maxbet)
        => $"{(maxbet / 1000).ToString("0.####", CultureInfo.InvariantCulture)}K";

    public static (string, string) GetBackgroundAndTextColor(MatchType matchType)
        => ColorCodeConst.ColorByGroupName.GetValueOrDefault(matchType, ColorCodeConst.ErrorColor);
}