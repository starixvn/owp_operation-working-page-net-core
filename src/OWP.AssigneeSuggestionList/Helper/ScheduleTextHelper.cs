using System.Globalization;
using OWP.AssigneeSuggestionList.Models;
using OWP.Enums;

namespace OWP.AssigneeSuggestionList.Helper
{
    public static class ScheduleTextHelper
    {
        public static string BuildScheduleTextGroupGame(DateTime eventDateTime, IList<int> owpMatchId, IList<MaxbetModel> maxbetModels)
        {
            var listMaxbet = new List<string>();
            foreach (var id in owpMatchId)
            {
                var maxbet = maxbetModels
                                            ?.FirstOrDefault(x => x.BettypeId == (int)EnumBettype.Handicap
                                            && x.OWPMatchId == id)
                                            ?.Maxbet;
                if (maxbet.HasValue)
                {
                    listMaxbet.Add($"{(maxbet.Value / 1000).ToString("0.####", CultureInfo.InvariantCulture)}K");
                }
            }

            return eventDateTime.Minute == 0
            ? $"V/{string.Join("+", listMaxbet)}"
            : $"{eventDateTime.ToString(@"hh\:mm")}/{listMaxbet}";
        }

        public static string BuildScheduleTextSingleGame(AssignmentModel assignment, IList<MaxbetModel> maxbetModels)
        {
            var maxbet = maxbetModels
                            ?.FirstOrDefault(x => x.BettypeId == (int)EnumBettype.Handicap
                            && x.OWPMatchId == assignment.OWPMatchId)
                            ?.Maxbet;
            var maxbetDisplay = maxbet.HasValue
                ? $"{(maxbet.Value / 1000).ToString("0.####", CultureInfo.InvariantCulture)}K"
                : string.Empty;
            return assignment.KOTime.Minutes == 0
            ? $"V/{maxbet}"
            : $"{assignment.KOTime.ToString(@"hh\:mm")}/{maxbet}";
        }
    }
}