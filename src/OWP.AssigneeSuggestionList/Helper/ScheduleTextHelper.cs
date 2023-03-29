using System.Globalization;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Helper
{
    public static class ScheduleTextHelper
    {
        public static string BuildScheduleTextGroupGame(DateTime eventDateTime, IList<AssignmentModel> assignments)
        {
            var owpMatchId = assignments?.Select(x => x.OWPMatchId).Distinct();
            var listMaxbet = new List<string>();
            foreach (var assignment in assignments)
            {
                if (assignment.Maxbet.HasValue)
                {
                    listMaxbet.Add($"{(assignment.Maxbet.Value / 1000).ToString("0.####", CultureInfo.InvariantCulture)}K");
                }
            }

            return eventDateTime.Minute == 0
            ? $"V/{string.Join("+", listMaxbet)}"
            : $"{eventDateTime.ToString(@"hh\:mm")}/{listMaxbet}";
        }

        public static string BuildScheduleTextSingleGame(AssignmentModel assignment)
        {
            var maxbetDisplay = assignment.Maxbet.HasValue
                ? $"{(assignment.Maxbet.Value / 1000).ToString("0.####", CultureInfo.InvariantCulture)}K"
                : string.Empty;
            return assignment.KOTime.Minutes == 0
            ? $"V/{assignment.Maxbet.Value}"
            : $"{assignment.KOTime.ToString(@"hh\:mm")}/{assignment.Maxbet.Value}";
        }
    }
}