using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;
using MatchType = OWP.AssigneeSuggestionList.Constant.MatchType;

namespace OWP.AssigneeSuggestionList.Services.RealMatch;

public interface LinkNGameService
{
    void Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedules,
        DateTime filterDate);
}

public class LinkNGameServiceImp : LinkNGameService
{

    private readonly WorkingScheduleService workingSchedule;

    public LinkNGameServiceImp(WorkingScheduleService workingSchedule)
    {
        this.workingSchedule = workingSchedule;
    }

    public void Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedules,
        DateTime filterDate)
    {
        LinkFourGamePlus(assignments, schedules, filterDate);
        LinkThreeGame(assignments, schedules, filterDate);
        LinkTwoGame(assignments, schedules, filterDate);
    }

    private void LinkTwoGame(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedules,
         DateTime filterDate)
    {
        var linkNGamePair = LinkGameConditionHelper
            .GroupGameByTimeIntervals(assignments)
            .Where(x => x.Value.Select(m => m.OWPMatchId).Distinct().Count() ==2)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        foreach (var game in linkNGamePair.Values)
        {
            game.ForEach(g => g.MatchType = MatchType.RealMatchLink2G);
        }

        var assignmentCell = new AssignmentCell
        {
            Color = ColorCodeConst.TWO_GAME,
            TextColor = ColorCodeConst.TEXT_TWO_GAME
        };
        UpdateSchedule(linkNGamePair, schedules, assignmentCell, filterDate, MatchType.RealMatchLink2G);
    }

    private void LinkFourGamePlus(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedules,
        DateTime filterDate)
    {
        var linkNGamePair = LinkGameConditionHelper
            .GroupGameByTimeIntervals(assignments)
            .Where(x => x.Value.Select(m => m.OWPMatchId).Distinct().Count() >= 4)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        foreach (var game in linkNGamePair.Values)
        {
            game.ForEach(g => g.MatchType = MatchType.RealMatchLink4G);
        }
        var assignmentCell = new AssignmentCell
        {
            Color = ColorCodeConst.FOUR_GAME,
            TextColor = ColorCodeConst.TEXT_FOUR_GAME
        };

        UpdateSchedule(linkNGamePair, schedules, assignmentCell, filterDate, MatchType.RealMatchLink4G);
    }

    private void LinkThreeGame(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedules,
        DateTime filterDate)
    {
        var linkNGamePair = LinkGameConditionHelper
            .GroupGameByTimeIntervals(assignments)
            .Where(x => x.Value.Select(m => m.OWPMatchId).Distinct().Count() == 3)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        foreach (var game in linkNGamePair.Values)
        {
            game.ForEach(g => g.MatchType = MatchType.RealMatchLink3G);
        }
        var assignmentCell = new AssignmentCell
        {
            Color = ColorCodeConst.THREE_GAME,
            TextColor = ColorCodeConst.TEXT_THREE_GAME
        };

        UpdateSchedule(linkNGamePair, schedules, assignmentCell, filterDate, MatchType.RealMatchLink3G);
    }

    private void UpdateSchedule(
        IDictionary<DateTime, List<AssignmentModel>> linkNGamePair,
        WorkingSchedule schedules,
        AssignmentCell assignmentCell,
        DateTime filterDate,
        MatchType matchType)
    {
        foreach (var eventDate in linkNGamePair.Keys)
        {
            var assignments = linkNGamePair[eventDate].DistinctBy(a => a.OWPMatchId).ToList();
            assignmentCell.Text = ScheduleHelper.BuildScheduleTextGroupGame(assignments);

            var etpkValue = FirstHalfHelper.IsFirstHalf(assignments) ? string.Empty : ETPKHelper.BuildEtPkTextDisplay(assignments);
            var timeRange = TimeRangeHelper.CalculateTimeRangeFT(eventDate, !string.IsNullOrEmpty(etpkValue));

            var scheduleUpdateModel = new ScheduleUpdateModel
            {
                FilterDate = filterDate,
                EtPk = new ETPKUpdateSchedule(etpkValue),
                AssignmentCell = assignmentCell,
                TimeRange = timeRange
            };
            var assignmentPresent = assignments.Find(x => !string.IsNullOrEmpty(ETPKHelper.DetectExtraTime(x.Remark, x.AssignmentNote, x.MinorBettypeId)))
                                    ?? assignments.MinBy(x => x.BettypeId);
            if (assignmentPresent is null)
            {
                continue;
            }

            assignmentPresent.GroupAssignmentModels = assignments.ToList();
            assignmentPresent.MatchType = matchType;
            workingSchedule.UpdateWorkingSchedule(scheduleUpdateModel, schedules, assignmentPresent);
        }
    }
}