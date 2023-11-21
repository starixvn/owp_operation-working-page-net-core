using AutoMapper;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;
using MatchType = OWP.AssigneeSuggestionList.Constant.MatchType;

namespace OWP.AssigneeSuggestionList.Services;

public interface SabaMatchServices
{
    void MapAssignmentsToWorkingSchedules(IList<WorkingSchedule> workingShifts, IList<AssignmentModel> assignmentModels, DateTime eventDate);

    void CategoryAssignee(List<WorkingSchedule> schedules, MatchInfo matchInfo);
}

public class SabaMatchesServicesImp : SabaMatchServices
{
    private readonly IMapper mapper;
    private readonly CategorizeSabaMatchServices categorizeSabaMatchServices;

    public SabaMatchesServicesImp(IMapper mapper, CategorizeSabaMatchServices categorizeSabaMatchServices)
    {
        this.mapper = mapper;
        this.categorizeSabaMatchServices = categorizeSabaMatchServices;
    }

    public void CategoryAssignee(List<WorkingSchedule> schedules, MatchInfo matchInfo)
    {
        matchInfo.MatchType = matchInfo.LeagueName.GetSabaBetType();
        categorizeSabaMatchServices.Categorize(schedules, matchInfo);
    }

    public void MapAssignmentsToWorkingSchedules(IList<WorkingSchedule> workingShifts, IList<AssignmentModel> assignmentModels, DateTime eventDate)
    {
        foreach (var workingShift in workingShifts)
        {
            var sabaAssignments = assignmentModels.Where(x =>
                !x.GameIsUsed &&
                x.UserId == workingShift.UserInfo.UserId)
                .OrderBy(x => x.EventDateTime)
                .ThenBy(x => x.OWPMatchId)
                .ThenBy(x => x.BettypeId)
                .DistinctBy(x => x.OWPMatchId)
                .ToList();

            CategorizeAndGroupSabaMatches(sabaAssignments);
            MarkGameIsUsed(sabaAssignments);
            workingShift.CategorizedAssignments.AddRange(sabaAssignments);

            MapAssignmentsToWorkingSchedule(workingShift, sabaAssignments, eventDate);
        }
    }

    private static void MarkGameIsUsed(List<AssignmentModel> sabaAssignments)
        => sabaAssignments.ForEach(g => g.GameIsUsed = true);

    public void CategorizeAndGroupSabaMatches(List<AssignmentModel> sabaAssignments)
    {
        CategorizeSabaMatches(sabaAssignments);
        GroupSaba5Mins(sabaAssignments);
        GroupSabaMix5minsGames(sabaAssignments);
        GroupSabaMultipleGames(sabaAssignments);
    }

    public static void CategorizeSabaMatches(List<AssignmentModel> sabaMatches)
    {
        sabaMatches.ForEach(x => { x.MatchType = x.LeagueName.GetSabaBetType(); });
    }

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

    public void GroupSabaMix5minsGames(List<AssignmentModel> sabaMatches)
    {
        var multipleSabaMatches = sabaMatches.Where(x => x.MatchType == MatchType.Saba5Mins)
                .GroupBy(s => s.EventDateTime.AddMinutes(-s.EventDateTime.Minute))
                .Where(g => g.Count() == 2)
                .SelectMany(g => g).ToList();

        sabaMatches.RemoveAll(multipleSabaMatches.Contains);

        var groupedMultipleMatches = multipleSabaMatches
            .GroupBy(x => x.EventDateTime.AddMinutes(-x.EventDateTime.Minute))
            .Select(y =>
            {
                var orderedAssignments = y.OrderBy(x => x.EventDateTime).ToList();
                var newAssignment = mapper.Map<AssignmentModel>(orderedAssignments[0]);
                newAssignment.MatchType = MatchType.SabaMix5mins;
                newAssignment.GroupAssignmentModels = orderedAssignments;
                return newAssignment;
            });

        sabaMatches.AddRange(groupedMultipleMatches);
    }

    public void GroupSabaMultipleGames(List<AssignmentModel> sabaMatches)
    {
        var multipleSabaMatches = sabaMatches.Where(x => x.MatchType.IsMultipleType()).ToList();
        sabaMatches.RemoveAll(multipleSabaMatches.Contains);

        var groupedMultipleMatches = multipleSabaMatches
            .GroupBy(x => x.EventDateTime)
            .Select(y =>
            {
                var orderedAssignments = y.OrderBy(x => x.EventDateTime).ToList();
                var newAssignment = mapper.Map<AssignmentModel>(orderedAssignments[0]);

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
            var isMultipleMatch = SabaMatch.AllowedMatchTypes.Contains(assignment.MatchType);
            var text = isMultipleMatch
                ? ScheduleHelper.BuildScheduleTextSabaGroupGame(assignment.GroupAssignmentModels)
                : ScheduleHelper.BuildScheduleTextSabaSingleGame(assignment);
            var (backgroundColor, textColor) = ScheduleHelper.GetBackgroundAndTextColor(assignment.MatchType);
            var assignmentCell = new AssignmentCell
            {
                Text = assignment.MatchType is MatchType.SabaOther ? "Error!" : text,
                TextColor = textColor,
                Color = backgroundColor,
                KoAssignment = assignment.EventDateTime
            };

            schedule.UpdateWorkingSchedule(assignment, assignmentCell, eventDate);
        }
    }
}
