using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services.RealMatch;

public interface XMinsGroupService
{
    Task Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedule,
        DateTime filterDate,
        IList<ParentMatchInfo> parentMatchInfo,
        List<int> owpIdExclude);
}

public class XMinsGroupServiceImp : XMinsGroupService
{

    private readonly SingleGameGroupService workingSchedule;

    public XMinsGroupServiceImp(SingleGameGroupService workingSchedule)
    {
        this.workingSchedule = workingSchedule;
    }

    public async Task Group(
        IList<AssignmentModel> assignments,
         WorkingSchedule schedule,
          DateTime filterDate,
           IList<ParentMatchInfo> parentMatchInfo,
        List<int> owpIdExclude)
    {
        var gameGroup = assignments?.Where(a => a.Is15Mins).ToList();
        owpIdExclude.AddRange(gameGroup?.Select(x => x.OWPMatchId).Distinct());

        await workingSchedule.Group(
            gameGroup,
            schedule,
            "[X_MINS]"
            , assignments15Mins => BuildScheduleUpdateModel(assignments15Mins, parentMatchInfo, filterDate));
    }

    public static ScheduleUpdateModel BuildScheduleUpdateModel(
        IList<AssignmentModel> assignments15Mins,
        IList<ParentMatchInfo> parentMatchInfo,
        DateTime filterDate)
    {
        var gameInfo = ScheduleHelper.GetMinorGameInfo(assignments15Mins);
        var parentGame = parentMatchInfo.FirstOrDefault(g => g.OwpMatchId == gameInfo.parentMatchId);
        var assignment = new AssignmentModel
        {
            KOTime = parentGame.KOTime,
            MaxbetHDP = gameInfo.maxbetHdp
        };

        var assignmentCell = new AssignmentCell
        {
            Color = ColorCodeConst.X_MINS,
            TextColor = ColorCodeConst.TEXT_X_MINS,
            Text = ScheduleHelper.BuildScheduleTextSingleGame(assignment)
        };
        var etPkValue = ETPKHelper.DetectExtraTime(parentGame?.Remark);

        return new ScheduleUpdateModel
        {
            FilterDate = filterDate,
            EtPk = new ETPKUpdateSchedule
            {
                IsHasEtPk = !string.IsNullOrEmpty(etPkValue),
                EtPkValue = etPkValue
            },
            AssignmentCell = assignmentCell,
            TimeRange = TimeRangeHelper.CalculateTimeRangeFT(gameInfo.eventDate, !string.IsNullOrEmpty(etPkValue))
        };
    }
}