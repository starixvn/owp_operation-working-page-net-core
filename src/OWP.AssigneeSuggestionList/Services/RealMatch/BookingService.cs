using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;
using OWP.Constants;

namespace OWP.AssigneeSuggestionList.Services.RealMatch;

public interface BookingService
{
    void Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedules,
        DateTime filterDate,
        IDictionary<int, List<AssignmentModel>> gameMapGameId);
}

public class BookingServiceImp : BookingService
{
    private readonly SpecialGameGroupService specialGameGroupService;

    public BookingServiceImp(SpecialGameGroupService specialGameGroupService)
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
                    .Where(a => a.MatchType is Constant.MatchType.Other && a.MinorBettypeId is Bettype.NextBooking or Bettype.TotalBooking)
                    .GroupBy(a => a.ParentMatchId.Value)
                    .ToDictionary(g => g.Key, g => g.ToList());

        foreach (var key in gameGroupPair.Keys)
        {
            gameGroupPair[key].ForEach(g => g.MatchType = Constant.MatchType.RealBooking);
        }

        specialGameGroupService.Group(
                    gameGroupPair,
                    schedules,
                    assignmentsCorner => BuildScheduleUpdateModel(gameMapGameId, assignmentsCorner, filterDate));
    }

    public static ScheduleUpdateModel BuildScheduleUpdateModel(IDictionary<int, List<AssignmentModel>> gameMapGameId,
     IList<AssignmentModel> assignmentsBooking,
      DateTime filterDate)
    {
        var textPostfix = ScheduleHelper.BuildTextSingleGamePostfixHdp(gameMapGameId, assignmentsBooking)
            ?? ScheduleHelper.BuildTextSingleGamePostfixOu(gameMapGameId, assignmentsBooking);

        var gameInfo = ScheduleHelper.GetGameInfo(assignmentsBooking);

        var tempEtPkValue = ETPKHelper.DetectExtraTime(gameInfo.remark, gameInfo.assignmentNote, gameInfo.minorBettypeId);
        var etPkValue = FirstHalfHelper.IsFirstHalf(assignmentsBooking) ? string.Empty : tempEtPkValue;

        return new ScheduleUpdateModel
        {
            FilterDate = filterDate,
            EtPk = new ETPKUpdateSchedule(etPkValue),
            AssignmentCell = new AssignmentCell
            {
                Color = ColorCodeConst.BOOKING,
                TextColor = ColorCodeConst.TEXT_BOOKING,
                Text = ScheduleHelper.BuildScheduleTextSingleGame(assignmentsBooking[0], textPostfix)
            },
            TimeRange = TimeRangeHelper.CalculateTimeRangeSingleGame(gameInfo.eventDate, gameInfo.bettypeIds, !string.IsNullOrEmpty(etPkValue))
        };
    }
}