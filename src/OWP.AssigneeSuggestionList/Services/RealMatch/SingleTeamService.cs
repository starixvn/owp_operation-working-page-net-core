using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;
using OWP.Constants;

namespace OWP.AssigneeSuggestionList.Services.RealMatch;

public interface SingleTeamService
{
    void Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedules,
        DateTime filterDate, IDictionary<int, List<AssignmentModel>> gameMapGameId);
}

public class SingleTeamServiceImp : SingleTeamService
{
    private readonly SingleGameGroupService workingSchedule;

    public SingleTeamServiceImp(SingleGameGroupService workingSchedule)
    {
        this.workingSchedule = workingSchedule;
    }

    public void Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedules,
        DateTime filterDate, IDictionary<int, List<AssignmentModel>> gameMapGameId)
    {
        var gameGroup = assignments
            .Where(a => a.MinorBettypeId is Bettype.HomeSingleTeam or Bettype.AwaySingleTeam)
            .ToList();
        foreach (var game in gameGroup)
        {
            game.MatchType = Constant.MatchType.RealSingleTeam;
        }
        workingSchedule.Group(
           gameGroup,
           schedules,
            assignment => BuildScheduleUpdateModel(gameMapGameId, assignment, filterDate));
    }

    public static ScheduleUpdateModel BuildScheduleUpdateModel(IDictionary<int, List<AssignmentModel>> gameMapGameId, IList<AssignmentModel> assignmentsSingleTeam, DateTime filterDate)
    {
        var textPostfix = ScheduleHelper.BuildTextSingleGamePostfixHdp(gameMapGameId, assignmentsSingleTeam);
        if (string.IsNullOrEmpty(textPostfix))
        {
            textPostfix = ScheduleHelper.BuildTextSingleGamePostfixOu(gameMapGameId, assignmentsSingleTeam);
        }

        var assignmentCell = new AssignmentCell
        {
            Color = ColorCodeConst.SINGLE_TEAM,
            TextColor = ColorCodeConst.TEXT_SINGLE_TEAM,
            Text = ScheduleHelper.BuildScheduleTextSingleGame(assignmentsSingleTeam[0], textPostfix)
        };

        var gameInfo = ScheduleHelper.GetGameInfo(assignmentsSingleTeam);
        var etPkValue = FirstHalfHelper.IsFirstHalf(assignmentsSingleTeam)
            ? string.Empty
            : ETPKHelper.DetectExtraTime(new AssignmentModel
            {
                Remark = gameInfo.remark,
                AssignmentNote = gameInfo.assignmentNote,
                MinorBettypeId = gameInfo.minorBettypeId
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