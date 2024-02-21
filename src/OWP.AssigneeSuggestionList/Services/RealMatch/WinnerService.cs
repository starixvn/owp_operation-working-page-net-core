using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;
using OWP.Constants;
using MatchType = OWP.AssigneeSuggestionList.Constant.MatchType;

namespace OWP.AssigneeSuggestionList.Services.RealMatch;

public interface WinnerService
{
    void Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedules,
        DateTime filterDate,
        IDictionary<int, List<AssignmentModel>> gameMapGameId,
        int bettype = Bettype.Winner);
}

public class WinnerServiceImp : WinnerService
{
    private readonly SpecialGroupService workingSchedule;

    public WinnerServiceImp(SpecialGroupService workingSchedule)
    {
        this.workingSchedule = workingSchedule;
    }

    public void Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedules,
        DateTime filterDate,
        IDictionary<int, List<AssignmentModel>> gameMapGameId,
        int bettype = Bettype.Winner)
    {
        var gameGroup = assignments
            .Where(a => a.MatchType == MatchType.Other && a.MinorBettypeId == bettype)
            .ToList();

        foreach (var game in gameGroup)
        {
            game.MatchType = MatchType.RealWinner;
        }
        workingSchedule.Group(
           gameGroup,
           schedules,
           assignmentWinner => BuildScheduleUpdateModel(gameMapGameId, assignmentWinner, filterDate));
    }

    public static ScheduleUpdateModel BuildScheduleUpdateModel(IDictionary<int, List<AssignmentModel>> gameMapGameId, IList<AssignmentModel> assignmentsWinner, DateTime filterDate)
    {
        var textPostfix = ScheduleHelper.BuildTextSingleGamePostfixHdp(gameMapGameId, assignmentsWinner);
        if (string.IsNullOrEmpty(textPostfix))
        {
            textPostfix = ScheduleHelper.BuildTextSingleGamePostfixOu(gameMapGameId, assignmentsWinner);
        }
        var (textColor, color) = GetColor(assignmentsWinner[0].MinorBettypeId ?? 0);
        var assignmentCell = new AssignmentCell
        {
            Color = color,
            TextColor = textColor,
            Text = ScheduleHelper.BuildScheduleTextSingleGame(assignmentsWinner[0], textPostfix)
        };

        var gameInfo = ScheduleHelper.GetGameInfo(assignmentsWinner);
        var etPkValue = FirstHalfHelper.IsFirstHalf(assignmentsWinner)
            ? string.Empty
            : ETPKHelper.DetectExtraTime(gameInfo.remark, gameInfo.assignmentNote, gameInfo.minorBettypeId);

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

    public static (string, string) GetColor(int minorBettypeId)
    {
        var colors = new Dictionary<int, (string, string)>
        {
            {Bettype.Winner, (ColorCodeConst.TEXT_WINNER, ColorCodeConst.WINNER)},
            {Bettype.NextRound, (ColorCodeConst.TEXT_NEXT_ROUND, ColorCodeConst.NEXT_ROUND)}
        };

        return colors.GetValueOrDefault(minorBettypeId);
    }
}