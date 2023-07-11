using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;


namespace OWP.AssigneeSuggestionList.Services.RealMatch;

public interface MJGroupService
{
    void Group(IList<AssignmentModel> assignments,
        WorkingSchedule schedules,
        DateTime filterDate,
        IDictionary<int, List<AssignmentModel>> gameMapGameId);
}

public class MJGroupServiceImp : MJGroupService
{
    private readonly SingleGameGroupService workingSchedule;

    public MJGroupServiceImp(SingleGameGroupService workingSchedule)
    {
        this.workingSchedule = workingSchedule;
    }
    public void Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedules,
        DateTime filterDate,
        IDictionary<int, List<AssignmentModel>> gameMapGameId)
    {
        var mJGroup = assignments.Where(a => a.IsMajorLeague)
            .ToList();
        mJGroup.ForEach(g => g.MatchType = Constant.MatchType.RealMj);
        workingSchedule.Group(
           mJGroup,
           schedules,
           assignment => BuildScheduleUpdateModel(gameMapGameId, assignment, filterDate));
    }

    public static ScheduleUpdateModel BuildScheduleUpdateModel(IDictionary<int, List<AssignmentModel>> gameMapGameId, IList<AssignmentModel> assignments, DateTime filterDate)
    {
        var textPostfix = ScheduleHelper.BuildTextSingleGamePostfixHdp(gameMapGameId, assignments);
        if (string.IsNullOrEmpty(textPostfix))
        {
            textPostfix = ScheduleHelper.BuildTextSingleGamePostfixOu(gameMapGameId, assignments);
        }
        var assignmentCell = new AssignmentCell
        {
            Color = ColorCodeConst.MJ,
            TextColor = ColorCodeConst.TEXT_MJ,
            Text = ScheduleHelper.BuildScheduleTextSingleGame(assignments.First(), textPostfix)
        };
        var gameInfo = ScheduleHelper.GetGameInfo(assignments);
        var etPkValue = ETPKHelper.DetectExtraTime(new AssignmentModel
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
            TimeRange = TimeRangeHelper.CalculateTimeRangeSingleGame(
                gameInfo.eventDate,
                gameInfo.bettypeIds,
                !string.IsNullOrEmpty(etPkValue))
        };
    }
}