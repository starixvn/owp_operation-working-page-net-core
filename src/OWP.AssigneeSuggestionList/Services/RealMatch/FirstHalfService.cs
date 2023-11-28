using MediatR;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;
using OWP.Enums;
using MatchType = OWP.AssigneeSuggestionList.Constant.MatchType;

namespace OWP.AssigneeSuggestionList.Services.RealMatch;

public interface FirstHalfService : IRequest<Unit>
{
    void Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedule,
        DateTime filterDate);
}

public class FirstHalfServiceImp : FirstHalfService
{
    private readonly SingleGameGroupService workingSchedule;

    public FirstHalfServiceImp(SingleGameGroupService workingSchedule)
    {
        this.workingSchedule = workingSchedule;
    }

    public void Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedule,
        DateTime filterDate)
    {
        var gameGroupPair = assignments
            .Where(g => g.MatchType == MatchType.Other)
            .GroupBy(x => x.OWPMatchId)
            .Where(x => x.All(a => a.BettypeId is (int)EnumBettype.FirstHalfHandicap or (int)EnumBettype.FirstHalfOverUnder))
            .ToDictionary(a => a.Key, a => a.ToList());

        var gameGroup = new List<AssignmentModel>();
        foreach (var game in gameGroupPair.Values)
        {
            game.ForEach(g => g.MatchType = MatchType.RealFirstHalf);
            gameGroup.AddRange(game);
        }

        workingSchedule.Group(gameGroup,
           schedule,
           assignment => BuildScheduleUpdateModel(assignment, filterDate),
           MatchType.RealMatchLink1G);
    }

    public static ScheduleUpdateModel BuildScheduleUpdateModel(IList<AssignmentModel> assignments, DateTime filterDate)
    {
        var assignmentCell = new AssignmentCell
        {
            Color = ColorCodeConst.FIRST_HALF,
            TextColor = ColorCodeConst.TEXT_FIRST_HALF,
            Text = ScheduleHelper.BuildScheduleTextSingleGame(assignments[0], string.Empty)
        };
        var gameInfo = ScheduleHelper.GetGameInfo(assignments);


        return new ScheduleUpdateModel
        {
            FilterDate = filterDate,
            EtPk = new ETPKUpdateSchedule(),
            AssignmentCell = assignmentCell,
            TimeRange = TimeRangeHelper.CalculateTimeRangeSingleGame(gameInfo.eventDate, gameInfo.bettypeIds, false)
        };
    }
}