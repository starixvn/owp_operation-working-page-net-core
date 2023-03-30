using OWP.AssigneeSuggestionList.Models;
using System.Globalization;

namespace OWP.AssigneeSuggestionList.Helper
{
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
                    listMaxbet.Add($"{(assignment.MaxbetHDP.Value / 1000).ToString("0.####", CultureInfo.InvariantCulture)}K");
                }
            }

            return eventDateTime.Minute == 0
            ? $"V/{string.Join("+", listMaxbet)}"
            : $"{eventDateTime.ToString(@"hh\:mm")}/{listMaxbet}";
        }

        public static string BuildScheduleTextSingleGame(AssignmentModel assignment)
        {
            var maxbetDisplay = assignment.MaxbetHDP.HasValue
                ? $"{(assignment.MaxbetHDP.Value / 1000).ToString("0.####", CultureInfo.InvariantCulture)}K"
                : string.Empty;
            return assignment.KOTime.Minutes == 0
            ? $"V/{maxbetDisplay}"
            : $"{assignment.KOTime.ToString(@"hh\:mm")}/{maxbetDisplay}";
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
    }
}