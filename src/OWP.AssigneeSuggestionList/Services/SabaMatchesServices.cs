using OWP.AssigneeSuggestionList.Models;
using OWP.Extensions;
using MatchType = OWP.AssigneeSuggestionList.Constant.MatchType;

namespace OWP.AssigneeSuggestionList.Services;

public interface SabaMatchesServices
{
    //IEnumerable<AssignmentModel> CategorizeAndGroupSabaMatches(IEnumerable<AssignmentModel> matches);
}

public class SabaMatchesServicesImp : SabaMatchesServices
{
    public void CalculateWorkingSchedule(IList<WorkingSchedule> workingShifts, IList<AssignmentModel> assignmentModels)
    {
        foreach (var workingShift in workingShifts)
        {
            var userAssignments = assignmentModels.Where(x => x.UserId == workingShift.UserInfo.UserId).ToList();
            var sabaAssignments = userAssignments.Where(x => x.IsSaba).ToList();

            var newSabaAssignments = CategorizeAndGroupSabaMatches(sabaAssignments);
        }
    }

    public List<AssignmentModel> CategorizeAndGroupSabaMatches(List<AssignmentModel> sabaAssignments)
    {
        CategorizeSabaMatches(sabaAssignments);
        GroupSaba5Mins(sabaAssignments);
        GroupSabaMultipleGames(sabaAssignments);

        return sabaAssignments;
    }

    public static void CategorizeSabaMatches(List<AssignmentModel> sabaMatches)
    {
        sabaMatches.ForEach(x => { x.MatchType = GetSabaBetType(x.LeagueName); });
    }

    public static MatchType GetSabaBetType(string leagueName)
        => leagueName switch
        {
            _ when leagueName.IsContains("M88 SABA") && leagueName.IsContains("- 16 Mins Play") => MatchType.SabaM8816Mins,
            _ when leagueName.IsContains("- 16 Mins Play") => MatchType.Saba16Mins,
            _ when leagueName.IsContains("- 6 Mins Play") => MatchType.SabaFutsal6Mins,
            _ when leagueName.IsContains("- 15 Mins Play") => MatchType.Saba15Mins,
            _ when leagueName.IsContains("PES ") && leagueName.IsContains("- 5 Mins Play") => MatchType.Saba5Mins,
            _ when leagueName.IsContains("- 20 Mins Play") => MatchType.Saba20Mins,
            _ => MatchType.SabaOther,
        };

    public static void GroupSaba5Mins(List<AssignmentModel> sabaMatches)
    {
        var saba5Mins = sabaMatches.Where(x => x.MatchType == MatchType.Saba5Mins).ToList();
        sabaMatches.RemoveAll(x => x.MatchType == MatchType.Saba5Mins);

        var groupedSaba5Mins = saba5Mins.GroupBy(
            x => (
                x.HomeName,
                x.LeagueName,
                x.AwayName,
                x.EventDateTime.AddMinutes(-x.EventDateTime.Minute)
            ))
            .Select(y => y.OrderBy(x => x.EventDateTime).First());

        sabaMatches.AddRange(groupedSaba5Mins);
    }

    public static void GroupSabaMultipleGames(List<AssignmentModel> sabaMatches)
    {
        MatchType[] multipleGameTypes = { MatchType.Saba15Mins, MatchType.Saba16Mins, MatchType.Saba20Mins };

        var multipleSabaMatches = sabaMatches.Where(x => multipleGameTypes.Contains(x.MatchType)).ToList();
        sabaMatches.RemoveAll(x => multipleSabaMatches.Contains(x));

        var groupedMultipleMatches = multipleSabaMatches
            .GroupBy(x => x.EventDateTime)
            .Select(y =>
            {
                var orderedAssignments = y.OrderBy(x => x.EventDateTime).ToList();
                var firstAssignment = orderedAssignments.First();
                firstAssignment.MatchType = GetMultipleMatchType(orderedAssignments.Count, firstAssignment.MatchType);
                firstAssignment.GroupAssignmentModels = orderedAssignments;

                return firstAssignment;
            });

        sabaMatches.AddRange(groupedMultipleMatches);
    }

    public static MatchType GetMultipleMatchType(int count, MatchType matchType)
        => (count, matchType) switch
        {
            (1, MatchType.Saba16Mins) => MatchType.Saba16Mins,
            (1, _) => MatchType.SabaLink1G,
            (2, _) => MatchType.SabaLink2G,
            (3, _) => MatchType.SabaLink3G,
            (4, _) => MatchType.SabaLink4G,
            _ => MatchType.SabaOther
        };
}
