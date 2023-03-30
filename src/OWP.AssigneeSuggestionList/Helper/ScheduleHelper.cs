using System.Globalization;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Helper
{
    public static class ScheduleHelper
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

        public static (DateTime eventDate, int owpMatchId, IList<int> bettypeIds) GetGameInfo(IList<AssignmentModel> assignments)
        {
            var gameInfo = assignments.First();
            var eventDate = gameInfo.EventDateTime;
            var owpMatchId = gameInfo.OWPMatchId;

            var bettypeIds = assignments.Select(x => x.BettypeId).Distinct().ToList();
            return (eventDate, owpMatchId, bettypeIds);
        }

        public static (DateTime eventDate, int owpMatchId, IList<int> bettypeIds, int? ParentMatchId) GetGameInfo15Mins(IList<AssignmentModel> assignments)
        {
            var parentMatchId = assignments.First().ParentMatchId;
            var (eventDate, owpMatchId, bettypeIds) = GetGameInfo(assignments);
            return (eventDate, owpMatchId, bettypeIds, parentMatchId);
        }
    }
}