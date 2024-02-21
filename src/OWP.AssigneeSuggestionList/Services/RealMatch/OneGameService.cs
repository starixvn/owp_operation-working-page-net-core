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
        DateTime filterDate, IDictionary<int, List<AssignmentModel>> gameMapGameId);
}

public class OneGameServiceImp : OneGameService
{
    private readonly SpecialGroupService workingSchedule;

    public OneGameServiceImp(SpecialGroupService workingSchedule)
    {
        this.workingSchedule = workingSchedule;
    }

    public void Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedules,
        DateTime filterDate,
        IDictionary<int, List<AssignmentModel>> gameMapGameId)
    {
        var oneGameGroupPair = LinkGameConditionHelper.GroupGame(assignments, numberGameInGroup => numberGameInGroup == 1);

        var oneGameGroup = new List<AssignmentModel>();
        foreach (var oneGame in oneGameGroupPair.Values)
        {
            oneGame.ForEach(g => g.MatchType = MatchType.RealMatchLink1G);
            oneGameGroup.AddRange(oneGame);
        }

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
            Text = ScheduleHelper.BuildScheduleTextSingleGame(assignments[0], textPostfix)
        };
        var gameInfo = ScheduleHelper.GetGameInfo(assignments);
        var etPkValue = FirstHalfHelper.IsFirstHalf(assignments)
        ? string.Empty
        : ETPKHelper.DetectExtraTime(gameInfo.remark, gameInfo.assignmentNote);

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