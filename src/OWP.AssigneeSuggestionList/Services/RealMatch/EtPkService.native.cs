using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services.RealMatch;

public interface EtPkService
{
    void Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedule,
        DateTime filterDate);
}

public class EtPkServiceImp : EtPkService
{
    private readonly SingleGameGroupService workingSchedule;

    public EtPkServiceImp(SingleGameGroupService workingSchedule)
    {
        this.workingSchedule = workingSchedule;
    }

    public void Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedule,
        DateTime filterDate)
    {
        var gameGroup = assignments?.Where(a =>
            !string.IsNullOrEmpty(ETPKHelper.DetectExtraTime(a.Remark, a.AssignmentNote, a.MinorBettypeId)) &&
            a.MatchType == Constant.MatchType.Other).ToList();

        workingSchedule.Group(
           gameGroup,
           schedule,
           assignment => BuildScheduleUpdateModel(assignment, filterDate));
    }

    public static ScheduleUpdateModel BuildScheduleUpdateModel(
        IList<AssignmentModel> assignments,
        DateTime filterDate)
    {
        var gameInfo = ScheduleHelper.GetGameInfo(assignments);

        var etPkValue = FirstHalfHelper.IsFirstHalf(assignments) ?
                string.Empty
                : ETPKHelper.DetectExtraTime(gameInfo.remark, gameInfo.assignmentNote);

        if (string.IsNullOrEmpty(etPkValue))
        {
            return null;
        }

        var assignmentCell = new AssignmentCell
        {
            Color = ColorCodeConst.ET_PK,
            TextColor = ColorCodeConst.TEXT_ET_PK,
            Text = etPkValue
        };

        return new ScheduleUpdateModel
        {
            FilterDate = filterDate,
            EtPk = new ETPKUpdateSchedule
            {
                IsHasEtPk = string.IsNullOrEmpty(etPkValue),
                EtPkValue = etPkValue
            },
            AssignmentCell = assignmentCell,
            TimeRange = (gameInfo.eventDate, gameInfo.eventDate)
        };
    }
}