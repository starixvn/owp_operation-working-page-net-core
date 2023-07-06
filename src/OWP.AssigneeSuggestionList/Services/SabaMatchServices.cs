using System.ComponentModel;
using AutoMapper;
using Newtonsoft.Json;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;
using OWP.Extensions;
using MatchType = OWP.AssigneeSuggestionList.Constant.MatchType;

namespace OWP.AssigneeSuggestionList.Services;

public interface SabaMatchServices
{
    void MapAssignmentsToWorkingSchedules(IList<WorkingSchedule> workingShifts, IList<AssignmentModel> assignmentModels, DateTime eventDate);
}

public class SabaMatchesServicesImp : SabaMatchServices
{
    private readonly IMapper mapper;

    public SabaMatchesServicesImp(IMapper mapper)
    {
        this.mapper = mapper;
    }

    public void MapAssignmentsToWorkingSchedules(IList<WorkingSchedule> workingShifts, IList<AssignmentModel> assignmentModels, DateTime eventDate)
    {
        foreach (var workingShift in workingShifts)
        {
            var userAssignments = assignmentModels.Where(x =>
                x.UserId == workingShift.UserInfo.UserId &&
                x.EndTime.CompareTo(workingShift.UserInfo.ShiftStartTime) >= 0 &&
                x.EventDateTime.CompareTo(workingShift.UserInfo.ShiftEndTime) < 0);

            var sabaAssignments = userAssignments
                .OrderBy(x => x.EventDateTime).ThenBy(x => x.OWPMatchId).ThenBy(x => x.BettypeId)
                .DistinctBy(x => x.OWPMatchId)
                .ToList();

            var newSabaAssignments = CategorizeAndGroupSabaMatches(sabaAssignments);
            workingShift.CategorizedAssignments.AddRange(newSabaAssignments);

            MapAssignmentsToWorkingSchedule(workingShift, newSabaAssignments, eventDate);
        }
    }

    public List<AssignmentModel> CategorizeAndGroupSabaMatches(List<AssignmentModel> sabaAssignments)
    {
        CategorizeSabaMatches(sabaAssignments);
        GroupSaba5Mins(sabaAssignments);
        GroupSaba5minsGames(sabaAssignments);
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
            _ => MatchType.SabaOther
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

    public  void GroupSaba5minsGames(List<AssignmentModel> sabaMatches)
    {
        var multipleSabaMatches = sabaMatches.Where(x => x.MatchType== MatchType.Saba5Mins)
                .GroupBy(s => s.EventDateTime.AddMinutes(-s.EventDateTime.Minute))
                .Where(g => g.Count() == 2)
                .SelectMany(g => g).ToList();
           
        sabaMatches.RemoveAll( multipleSabaMatches.Contains);

        var groupedMultipleMatches = multipleSabaMatches
            .GroupBy(x => x.EventDateTime.AddMinutes(-x.EventDateTime.Minute))
            .Select(y =>
            {
                var orderedAssignments = y.OrderBy(x => x.EventDateTime).ToList();
                var newAssignment = mapper.Map<AssignmentModel>(orderedAssignments.First());
                newAssignment.MatchType = MatchType.SabaMix5mins;
                newAssignment.GroupAssignmentModels = orderedAssignments;
                return newAssignment;
            });

        sabaMatches.AddRange(groupedMultipleMatches);
    }

    public  void GroupSabaMultipleGames(List<AssignmentModel> sabaMatches)
    {
        MatchType[] multipleGameTypes = { MatchType.Saba15Mins, MatchType.Saba16Mins, MatchType.Saba20Mins };

        var multipleSabaMatches = sabaMatches.Where(x => multipleGameTypes.Contains(x.MatchType)).ToList();
        sabaMatches.RemoveAll(multipleSabaMatches.Contains);

        var groupedMultipleMatches = multipleSabaMatches
            .GroupBy(x => x.EventDateTime)
            .Select(y =>
            {
                var orderedAssignments = y.OrderBy(x => x.EventDateTime).ToList();
                var newAssignment = mapper.Map<AssignmentModel>(orderedAssignments.First()); 
           
                newAssignment.MatchType = GetMultipleMatchType(orderedAssignments.Count, newAssignment.MatchType);
                newAssignment.GroupAssignmentModels = orderedAssignments;

                return newAssignment;
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

    public static void MapAssignmentsToWorkingSchedule(WorkingSchedule schedule, List<AssignmentModel> assignments, DateTime eventDate)
    {
        foreach (var assignment in assignments)
        {
            var isMultipleMatch =
                assignment.MatchType is MatchType.SabaLink2G or MatchType.SabaLink3G or MatchType.SabaLink4G or MatchType.SabaMix5mins;
            var text = isMultipleMatch
                ? ScheduleHelper.BuildScheduleTextSabaGroupGame(assignment.GroupAssignmentModels)
                : ScheduleHelper.BuildScheduleTextSabaSingleGame(assignment);
            var (backgroundColor, textColor) = ScheduleHelper.GetBackgroundAndTextColor(assignment.MatchType);
            var assignmentCell = new AssignmentCell
            {
                Text = assignment.MatchType is MatchType.SabaOther ? "Error!" : text,
                TextColor = textColor,
                Color = backgroundColor
            };

            schedule.UpdateWorkingSchedule(assignment, assignmentCell, eventDate);
        }
    }
}
