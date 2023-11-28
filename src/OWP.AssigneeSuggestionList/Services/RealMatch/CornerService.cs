using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;

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
        var textPostfix = ScheduleHelper.BuildTextSingleGamePostfixHdp(gameMapGameId, assignmentsCorner);
        if (string.IsNullOrEmpty(textPostfix))
        {
            textPostfix = ScheduleHelper.BuildTextSingleGamePostfixOu(gameMapGameId, assignmentsCorner);
        }

        var gameInfo = ScheduleHelper.GetGameInfo(assignmentsCorner);
        var tempEtPkValue = ETPKHelper.DetectExtraTime(new AssignmentModel
        {
            Remark = gameInfo.remark,
            AssignmentNote = gameInfo.assignmentNote,
            MinorBettypeId = gameInfo.minorBettypeId
        });

        var etPkValue = (FirstHalfHelper.IsFirstHalf(assignmentsCorner) || string.Equals(tempEtPkValue, "PK", StringComparison.OrdinalIgnoreCase))
        ? string.Empty
            : tempEtPkValue;

        return new ScheduleUpdateModel
        {
            FilterDate = filterDate,
            EtPk = new ETPKUpdateSchedule
            {
                IsHasEtPk = !string.IsNullOrEmpty(etPkValue),
                EtPkValue = etPkValue
            },
            AssignmentCell = new()
            {
                Color = ColorCodeConst.CORNER,
                TextColor = ColorCodeConst.TEXT_CORNER,
                Text = ScheduleHelper.BuildScheduleTextSingleGame(assignmentsCorner[0], textPostfix)
            },
            TimeRange = TimeRangeHelper.CalculateTimeRangeSingleGame(gameInfo.eventDate, gameInfo.bettypeIds, !string.IsNullOrEmpty(etPkValue))
        };
    }
}