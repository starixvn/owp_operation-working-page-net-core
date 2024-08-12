using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;
using OWP.Constants;

namespace OWP.AssigneeSuggestionList.Services.RealMatch;

public interface CornerService
{
    void Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedules,
        DateTime filterDate,
        IDictionary<int, List<AssignmentModel>> gameMapGameId);
}

public class CornerServiceImp : CornerService
{
    private readonly SpecialGameGroupService specialGameGroupService;

    public CornerServiceImp(SpecialGameGroupService specialGameGroupService)
    {
        this.specialGameGroupService = specialGameGroupService;
    }

    public void Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedules,
        DateTime filterDate,
        IDictionary<int, List<AssignmentModel>> gameMapGameId)
    {
        var gameGroupPair = assignments
            .Where(a => a.IsCornerMatch && a.MatchType is Constant.MatchType.Other)
            .GroupBy(a => a.ParentMatchId.Value)
            .ToDictionary(g => g.Key, g => g.ToList());

        foreach (var key in gameGroupPair.Keys)
        {
            gameGroupPair[key].ForEach(g => g.MatchType = Constant.MatchType.RealCorner);
        }

        specialGameGroupService.Group(
            gameGroupPair,
            schedules,
            assignmentsCorner => BuildScheduleUpdateModel(gameMapGameId, assignmentsCorner, filterDate));
    }

    public static ScheduleUpdateModel BuildScheduleUpdateModel(
        IDictionary<int, List<AssignmentModel>> gameMapGameId,
        IList<AssignmentModel> assignmentsCorner,
        DateTime filterDate)
    {
        var textPostfix = ScheduleHelper.BuildTextSingleGamePostfixHdp(gameMapGameId, assignmentsCorner, Bettype.NoOfCorner);
        if (string.IsNullOrEmpty(textPostfix))
        {
            textPostfix = ScheduleHelper.BuildTextSingleGamePostfixOu(gameMapGameId, assignmentsCorner, Bettype.NoOfCorner);
        }

        var gameInfo = ScheduleHelper.GetGameInfo(assignmentsCorner);
        var tempEtPkValue = ETPKHelper.DetectExtraTime(gameInfo.remark, gameInfo.assignmentNote, gameInfo.minorBettypeId);

        var etPkValue = FirstHalfHelper.IsFirstHalf(assignmentsCorner)
            ? string.Empty
            : tempEtPkValue;

        return new ScheduleUpdateModel
        {
            FilterDate = filterDate,
            EtPk = new ETPKUpdateSchedule(etPkValue),
            AssignmentCell = new AssignmentCell
            {
                Color = ColorCodeConst.CORNER,
                TextColor = ColorCodeConst.TEXT_CORNER,
                Text = ScheduleHelper.BuildScheduleTextSingleGame(assignmentsCorner[0], textPostfix)
            },
            TimeRange = TimeRangeHelper.CalculateTimeRangeSingleGame(gameInfo.eventDate, gameInfo.bettypeIds, !string.IsNullOrEmpty(etPkValue))
        };
    }
}