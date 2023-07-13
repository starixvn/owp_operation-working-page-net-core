using OWP.AssigneeSuggestionList.Models;
using OWP.Enums;

namespace OWP.AssigneeSuggestionList.Helper;

public static class LinkTwoGameConditionHelper
{
    public static IDictionary<DateTime, List<AssignmentModel>> Group(
       IList<AssignmentModel> assignments)
    {
        var group2GameDictionary = new Dictionary<DateTime, List<AssignmentModel>>();
        GroupTwoGameSameKo(group2GameDictionary, assignments);
        GroupTwoGameKoInRange10Mins(group2GameDictionary, assignments);
        return group2GameDictionary;
    }

    private static void GroupTwoGameSameKo(
        IDictionary<DateTime, List<AssignmentModel>> group2GameDictionary,
        IList<AssignmentModel> assignments)
    {
        var assignmentSameKo = assignments.Where(LinkGameConditionHelper.CanGroupNGame)
            .GroupBy(a => a.EventDateTime)
            .Where(g => g.Select(x => x.OWPMatchId).Distinct().Count() == 2)
            .SelectMany(g => g.Select(x => x))
            .ToList();

        assignmentSameKo.ForEach(g => g.MatchType = Constant.MatchType.RealMatchLink2G);

        var assignmentSameKoDictionary = assignmentSameKo.GroupBy(x => x.EventDateTime)
            .ToDictionary(x => x.Key, x => x.ToList());

        foreach (var assignment in assignmentSameKoDictionary)
        {
            group2GameDictionary.Add(assignment.Key, assignment.Value);
        }
    }

    private static void GroupTwoGameKoInRange10Mins(
        IDictionary<DateTime, List<AssignmentModel>> group2GameDictionary,
        IList<AssignmentModel> assignments)
    {
        var assignmentOther = assignments
            .Where(a => a.MatchType != Constant.MatchType.RealMatchLink2G && LinkGameConditionHelper.CanGroupNGame(a))
            .OrderBy(x => x.EventDateTime);

        var countAssignment = assignmentOther.Count();
        for (var i = 0; i < countAssignment; i++)
        {
            if (assignments[i].MatchType == Constant.MatchType.RealMatchLink2G)
            {
                continue;
            }
            for (var j = i + 1; j < countAssignment; j++)
            {
                var durations = assignments[i].EventDateTime.Subtract(assignments[j].EventDateTime).Duration();

                if (durations <= TimeSpan.FromMinutes(10))
                {
                    var eventDate = UtilsHelper.SelectTime(assignments[i].EventDateTime, assignments[j].EventDateTime);
                    group2GameDictionary.TryAdd(eventDate, new List<AssignmentModel>{
                        assignments[i],
                        assignments[j]
                    });
                    assignments[i].MatchType = Constant.MatchType.RealMatchLink2G;
                    assignments[j].MatchType = Constant.MatchType.RealMatchLink2G;
                    break;
                }
            }
        }
    }
}