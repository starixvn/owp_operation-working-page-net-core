using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;
using OWP.Constants;

namespace OWP.AssigneeSuggestionList.Services.RealMatch;

public interface XMinsGroupService
{
    void Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedule,
        DateTime filterDate,
        int bettype = Bettype.Specific15Mins);
}

public class XMinsGroupServiceImp : XMinsGroupService
{
    private readonly SpecialGroupService workingSchedule;

    public XMinsGroupServiceImp(SpecialGroupService workingSchedule)
    {
        this.workingSchedule = workingSchedule;
    }

    public void Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedule,
        DateTime filterDate,
        int bettype = Bettype.Specific15Mins)
    {
        var gameGroup = assignments.Where(a => a.MatchType == Constant.MatchType.Other && a.MinorBettypeId == bettype).ToList();

        gameGroup.ForEach(game => game.MatchType = Constant.MatchType.Real15MOU);
        workingSchedule.Group(
           gameGroup,
           schedule,
           assignments15Mins => BuildScheduleUpdateModel(assignments15Mins, filterDate));
    }

    public static ScheduleUpdateModel BuildScheduleUpdateModel(IList<AssignmentModel> assignments15Mins, DateTime filterDate)
    {
        var gameInfo = ScheduleHelper.GetGameInfo(assignments15Mins);
        var assignment = new AssignmentModel
        {
            KOTime = assignments15Mins[0].KOTime,
            MaxbetHDP = gameInfo.maxbetHdp
        };
        var (textColor, color) = GetColor(assignments15Mins[0].MinorBettypeId ?? 0);

        return new ScheduleUpdateModel
        {
            FilterDate = filterDate,
            EtPk = new ETPKUpdateSchedule(),
            AssignmentCell = new AssignmentCell
            {
                Color = color,
                TextColor = textColor,
                Text = ScheduleHelper.BuildScheduleTextSingleGame(assignment, string.Empty)
            },
            TimeRange = TimeRangeHelper.CalculateTimeRangeFT(gameInfo.eventDate, false)
        };
    }

    private static (string, string) GetColor(int minorBettypeId)
    {
        var colors = new Dictionary<int, (string, string)>
        {
            {Bettype.Specific15Mins, (ColorCodeConst.TEXT_X_MINS, ColorCodeConst.X_MINS)},
            {Bettype.Specific15MinsCorners, (ColorCodeConst.TEXT_X_MINS, ColorCodeConst.CR_15_MINS)},
            {Bettype.Specific15MinsTotalBookings, (ColorCodeConst.TEXT_X_MINS, ColorCodeConst.TB_15_MINS)}
        };

        return colors.GetValueOrDefault(minorBettypeId);
    }
}