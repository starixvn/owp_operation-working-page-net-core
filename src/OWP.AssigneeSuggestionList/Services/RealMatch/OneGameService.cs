using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;
using MatchType = OWP.AssigneeSuggestionList.Constant.MatchType;

namespace OWP.AssigneeSuggestionList.Services.RealMatch;

public interface OneGameService
{
    void Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedules,
        DateTime filterDate,
        List<int> owpIdExclude, IDictionary<int, List<AssignmentModel>> gameMapGameId);
}

public class OneGameServiceImp : OneGameService
{
    private readonly SingleGameGroupService workingSchedule;

    public OneGameServiceImp(SingleGameGroupService workingSchedule)
    {
        this.workingSchedule = workingSchedule;
    }

    public void Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedules,
        DateTime filterDate,
        List<int> owpIdExclude,
        IDictionary<int, List<AssignmentModel>> gameMapGameId)
    {
        var oneGameGroupPair = LinkGameConditionHelper.GroupGame(assignments, numberGameInGroup => numberGameInGroup == 1);

        var oneGameGroup = new List<AssignmentModel>();
        foreach (var assignment in oneGameGroupPair.Values)
        {
            oneGameGroup.AddRange(assignment);
        }

        owpIdExclude.AddRange(oneGameGroup.Select(x => x.OWPMatchId).Distinct());

        workingSchedule.Group(
           oneGameGroup,
           schedules,
           assignment => BuildScheduleUpdateModel(gameMapGameId, assignment, filterDate),
           MatchType.RealMatchLink1G);
    }

    public static ScheduleUpdateModel BuildScheduleUpdateModel(
    IDictionary<int, List<AssignmentModel>> gameMapGameId,
    IList<AssignmentModel> assignments, DateTime filterDate)
    {
        var textPostfix = ScheduleHelper.BuildTextSingleGamePostfixHdp(gameMapGameId, assignments);
        if (string.IsNullOrEmpty(textPostfix))
        {
            textPostfix = ScheduleHelper.BuildTextSingleGamePostfixOu(gameMapGameId, assignments);
        }
        var assignmentCell = new AssignmentCell
        {
            Color = ColorCodeConst.ONE_GAME,
            TextColor = ColorCodeConst.TEXT_ONE_GAME,
            Text = ScheduleHelper.BuildScheduleTextSingleGame(assignments.First(), textPostfix)
        };
        var gameInfo = ScheduleHelper.GetGameInfo(assignments);
        var etPkValue = FirstHalfHelper.IsFirstHalf(assignments)
        ? string.Empty
        : ETPKHelper.DetectExtraTime(new AssignmentModel
        {
            Remark = gameInfo.remark,
            AssignmentNote = gameInfo.assignmentNote
        });

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