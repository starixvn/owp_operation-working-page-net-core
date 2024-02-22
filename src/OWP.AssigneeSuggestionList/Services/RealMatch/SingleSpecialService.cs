using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;
using OWP.Constants;
using MatchType = OWP.AssigneeSuggestionList.Constant.MatchType;

namespace OWP.AssigneeSuggestionList.Services.RealMatch;

public interface SingleSpecialService
{
    void Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedules,
        DateTime filterDate,
        IDictionary<int, List<AssignmentModel>> gameMapGameId,
        params int?[] bettype);
}

public class WinnerServiceImp : SingleSpecialService
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
        params int?[] bettype)
    {
        var gameGroup = assignments
            .Where(a => a.MatchType == MatchType.Other && bettype.Contains(a.MinorBettypeId))
            .ToList();

        var matchType = GetMatchType(bettype[0]);
        gameGroup.ForEach(game => game.MatchType = matchType);

        workingSchedule.Group(
           gameGroup,
           schedules,
           assignmentWinner => BuildScheduleUpdateModel(gameMapGameId, assignmentWinner, filterDate));
    }

    public static ScheduleUpdateModel BuildScheduleUpdateModel(IDictionary<int, List<AssignmentModel>> gameMapGameId, IList<AssignmentModel> assignmentsWinner, DateTime filterDate)
    {
        var textPostfix = ScheduleHelper.BuildTextSingleGamePostfixHdp(gameMapGameId, assignmentsWinner)
            ?? ScheduleHelper.BuildTextSingleGamePostfixOu(gameMapGameId, assignmentsWinner);

        var (textColor, color) = GetColor(assignmentsWinner[0].MinorBettypeId ?? 0);
        var gameInfo = ScheduleHelper.GetGameInfo(assignmentsWinner);
        string etPkValue = GetETPK(assignmentsWinner, gameInfo.minorBettypeId, gameInfo.remark, gameInfo.assignmentNote);

        return new ScheduleUpdateModel
        {
            FilterDate = filterDate,
            EtPk = new ETPKUpdateSchedule(etPkValue),
            AssignmentCell = new AssignmentCell
            {
                Color = color,
                TextColor = textColor,
                Text = ScheduleHelper.BuildScheduleTextSingleGame(assignmentsWinner[0], textPostfix)
            },
            TimeRange = TimeRangeHelper.CalculateTimeRangeSingleGame(gameInfo.eventDate, gameInfo.bettypeIds, !string.IsNullOrEmpty(etPkValue))
        };
    }

    private static string GetETPK(IList<AssignmentModel> assignmentsWinner, int? minorBettypeId, string remark, string assignmentNote)
    {
        return FirstHalfHelper.IsFirstHalf(assignmentsWinner)
                    ? string.Empty
                    : ETPKHelper.DetectExtraTime(remark, assignmentNote, minorBettypeId);
    }

    public static (string, string) GetColor(int minorBettypeId) => minorBettypeId switch
    {
        Bettype.Winner => (ColorCodeConst.TEXT_WINNER, ColorCodeConst.WINNER),
        Bettype.NextRound => (ColorCodeConst.TEXT_NEXT_ROUND, ColorCodeConst.NEXT_ROUND),
        Bettype.HomeSingleTeam or Bettype.AwaySingleTeam => (ColorCodeConst.TEXT_SINGLE_TEAM, ColorCodeConst.SINGLE_TEAM),
        Bettype.Specific15MinsCorners => (ColorCodeConst.TEXT_X_MINS, ColorCodeConst.CR_15_MINS),
        Bettype.Specific15MinsTotalBookings => (ColorCodeConst.TEXT_X_MINS, ColorCodeConst.TB_15_MINS),
        _ => (string.Empty, string.Empty)
    };

    public static MatchType GetMatchType(int? minorBettypeId) => minorBettypeId switch
    {
        Bettype.Winner or Bettype.NextRound => MatchType.RealWinner,
        Bettype.HomeSingleTeam or Bettype.AwaySingleTeam => MatchType.RealSingleTeam,
        Bettype.Specific15Mins or Bettype.Specific15MinsTotalBookings or Bettype.Specific15MinsCorners => MatchType.Real15MOU,
        _ => MatchType.Other
    };
}