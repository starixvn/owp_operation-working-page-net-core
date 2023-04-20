using MediatR;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;
using OWP.Enums;


namespace OWP.AssigneeSuggestionList.Services.RealMatch;

public interface FirstHalfService : IRequest<Unit>
{
    void Group(
        IList<AssignmentModel> assignments,
         WorkingSchedule schedule,
          DateTime filterDate,
        List<int> owpIdExclude);
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
        DateTime filterDate,
        List<int> owpIdExclude)
    {
        var gameGroupPair = assignments
            .GroupBy(x => x.OWPMatchId)
            .Where(x => x.All(a => a.BettypeId is (int)EnumBettype.FirstHalfHandicap or (int)EnumBettype.FirstHalfOverUnder))
            .ToDictionary(a => a.Key, a => a.ToList());

        var gameGroup = new List<AssignmentModel>();
        owpIdExclude.AddRange(gameGroupPair.Keys);
        foreach (var item in gameGroupPair)
        {
            gameGroup.AddRange(item.Value);

        }
        workingSchedule.Group(gameGroup,
           schedule,
           _ => BuildScheduleUpdateModel(assignments, filterDate));
    }

    public static ScheduleUpdateModel BuildScheduleUpdateModel(IList<AssignmentModel> assignments, DateTime filterDate)
    {
        var assignmentCell = new AssignmentCell
        {
            Color = ColorCodeConst.FIRST_HALF,
            TextColor = ColorCodeConst.TEXT_FIRST_HALF,
            Text = ScheduleHelper.BuildScheduleTextSingleGame(assignments.First())
        };
        var gameInfo = ScheduleHelper.GetGameInfo(assignments);
        var etPkValue = ETPKHelper.DetectExtraTime(gameInfo.remark);

        return new ScheduleUpdateModel
        {
            FilterDate = filterDate,
            EtPk = new ETPKUpdateSchedule
            {
                IsHasEtPk = !string.IsNullOrEmpty(etPkValue),
                EtPkValue = etPkValue
            },
            AssignmentCell = assignmentCell,
            TimeRange = TimeRangeHelper.CalculateTimeRangeSingleGame(gameInfo.eventDate, gameInfo.bettypeIds, !string.IsNullOrEmpty(etPkValue))
        };
    }
}