using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;
using OWP.Constants;
using OWP.Extensions;
using System.Collections.Immutable;
using System.Globalization;
using MatchType = OWP.AssigneeSuggestionList.Constant.MatchType;

namespace OWP.AssigneeSuggestionList.Helper;

public static class ScheduleHelper
{
    public static string BuildScheduleTextSabaGroupGame(IList<AssignmentModel> assignments)
    {
        var eventDateTime = assignments[0].EventDateTime;
        var labelList = assignments.GroupBy(g => g.MatchType)
            .Select(g => g.Count() == 1
                ? $"{MatchTypeLabel.MatchTypeMapLabel[g.Key]}"
                : $"{MatchTypeLabel.MatchTypeMapLabel[g.Key]}*{g.Count()}").ToList();


        return eventDateTime.Minute == 0
        ? $"*V/{string.Join("+", labelList)}"
        : $"*{eventDateTime:hh\\:mm}/{string.Join("+", labelList)}";
    }

    public static string BuildScheduleTextGroupGame(IList<AssignmentModel> assignments)
    {
        var kOTimeText = BuildKoTextGroupGame(assignments);
        var mBText = BuildMbTextGroupGame(assignments);
        return string.IsNullOrEmpty(mBText)
        ? $"{kOTimeText}"
        : $"{kOTimeText}/{mBText}";
    }

    private static string BuildKoTextGroupGame(IList<AssignmentModel> assignments)
    {
        var eventDateTime = assignments.Select(x => x.EventDateTime).Distinct().ToList();

        return eventDateTime.TrueForAll(x => x.Minute == 0)
        ? "V"
        : string.Join("+", eventDateTime.Select(x => x.Minute == 0 ? "V" : $"{x:hh\\:mm}"));
    }

    private static string BuildMbTextGroupGame(IList<AssignmentModel> assignments)
    {
        var listMaxbet = assignments
            .Where(a => a.MaxbetHDP.HasValue)
            .Select(a => $"{FormatMaxbet(a.MaxbetHDP.Value)}")
            .Distinct()
            .ToList();
        return string.Join("+", listMaxbet);
    }

    public static string BuildScheduleTextSabaSingleGame(AssignmentModel assignment)
    {
        if (assignment.MatchType == MatchType.Saba5Mins)
        {
            return BuildScheduleText5MinsSabaSingleGame(assignment);
        }
        return assignment.KOTime.Minutes == 0
        ? "*V"
        : $"*{assignment.KOTime.ToKOTimeWithoutSuffix()}";
    }


    private static string BuildScheduleText5MinsSabaSingleGame(AssignmentModel assignment)
    => assignment.KOTime.Minutes == 0
        ? "*V/5"
        : $"*{assignment.KOTime.ToKOTimeWithoutSuffix()}/5";

    public static string BuildScheduleTextSingleGame(AssignmentModel assignment, string textPostfix)
    {
        var maxbetDisplay = assignment.MaxbetHDP.HasValue
                ? $"{FormatMaxbet(assignment.MaxbetHDP.Value)}"
            : string.Empty;
        var kOTimeText = assignment.KOTime.ToKOTimeWithoutSuffix();

        var result = assignment.KOTime.Minutes == 0
        ? "V"
        : $"{kOTimeText}";

        if (!string.IsNullOrEmpty(maxbetDisplay))
        {
            result += $"/{maxbetDisplay}";
        }

        if (!string.IsNullOrEmpty(textPostfix))
        {
            result += $"{textPostfix}";
        }
        return result;
    }

    public static string BuildTextSingleGamePostfixHdp(IDictionary<int, List<AssignmentModel>> gameMapGameId,
        IList<AssignmentModel> assignments)
    {
        string prefix = null;
        var assignmentHdp = assignments.FirstOrDefault(a => a.BettypeId == Bettype.Handicap);
        if (assignmentHdp is not null)
        {
            prefix = "*H";
            if (gameMapGameId[assignmentHdp.OWPMatchId].Exists(a =>
                    a.OWPMatchId == assignmentHdp.OWPMatchId
                    && a.BettypeId == Bettype.OverUnder
                    && a.UserId == assignmentHdp.UserId))
            {
                prefix = string.Empty;
            }
        }

        return prefix;
    }

    public static string BuildTextSingleGamePostfixOu(IDictionary<int, List<AssignmentModel>> gameMapGameId,
        IList<AssignmentModel> assignments)
    {
        string prefix = null;
        var assignmentOu = assignments.FirstOrDefault(a => a.BettypeId == Bettype.OverUnder);
        if (assignmentOu is not null)
        {
            prefix = "*O";
            if (gameMapGameId[assignmentOu.OWPMatchId].Exists(a =>
                    a.OWPMatchId == assignmentOu.OWPMatchId
                    && a.BettypeId == Bettype.Handicap
                    && a.UserId == assignmentOu.UserId))
            {
                prefix = string.Empty;
            }
        }

        return prefix;
    }

    public static (
        DateTime eventDate,
         IList<int> bettypeIds,
          int? minorBettypeId,
           string remark,
            decimal? maxbetHdp,
             string assignmentNote) GetGameInfo(IList<AssignmentModel> assignments)
    {
        var firstAssignment = assignments[0];
        var bettypeIds = assignments.Select(x => x.BettypeId).Distinct().ToList();

        return (
            firstAssignment.EventDateTime,
            bettypeIds,
            firstAssignment.MinorBettypeId,
            firstAssignment.Remark,
            firstAssignment.MaxbetHDP,
            firstAssignment.AssignmentNote);
    }

    public static string FormatMaxbet(decimal maxbet)
        => $"{(maxbet / 1000).ToString("0.####", CultureInfo.InvariantCulture)}K";

    public static (string, string) GetBackgroundAndTextColor(MatchType matchType)
        => ColorCodeConst.ColorByGroupName.GetValueOrDefault(matchType, ColorCodeConst.ErrorColor);
}