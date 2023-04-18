using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services.RealMatch;

public interface EtPkService
{
    void Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedule,
        DateTime filterDate,
        IList<int> owpMatchIdExclude);
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
         DateTime filterDate,
          IList<int> owpMatchIdExclude)
    {
        var gameGroup = assignments?.Where(a =>
            !string.IsNullOrEmpty(ETPKHelper.DetectExtraTime(a.Remark)) &&
            !owpMatchIdExclude.Contains(a.OWPMatchId)).ToList();

        workingSchedule.Group(
           gameGroup,
           schedule,
           assignments => BuildScheduleUpdateModel(assignments, filterDate));
    }

    public static ScheduleUpdateModel BuildScheduleUpdateModel(
        IList<AssignmentModel> assignments,
        DateTime filterDate)
    {
        var gameInfo = ScheduleHelper.GetGameInfo(assignments);

        var etPkValue = ETPKHelper.DetectExtraTime(gameInfo.remark);

        var assignmentCell = new AssignmentCell
        {
            Color = ColorCodeConst.ET_PK,
            TextColor = ColorCodeConst.TEXT_ET_PK,
            Text = etPkValue
        };

        return new()
        {
            FilterDate = filterDate,
            EtPk = new ETPKUpdateSchedule
            {
                IsHasEtPk = true,
                EtPkValue = etPkValue
            },
            AssignmentCell = assignmentCell,
            TimeRange = (gameInfo.eventDate, gameInfo.eventDate)
        };
    }
}