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
    private readonly SingleGameGroupService workingSchedule;

    public BookingServiceImp(SingleGameGroupService workingSchedule)
    {
        this.workingSchedule = workingSchedule;
    }

    public void Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedules,
        DateTime filterDate,
        IDictionary<int, List<AssignmentModel>> gameMapGameId)
    {
        var gameGroup = assignments
            .Where(a => a.MinorBettypeId is Bettype.FifteenMinsBookings or Bettype.NextBooking or Bettype.TotalBooking)
            .ToList();

        foreach (var game in gameGroup)
        {
            game.MatchType = Constant.MatchType.RealBooking;
        }

        workingSchedule.Group(
           gameGroup,
           schedules,
            assignmentBooking => BuildScheduleUpdateModel(assignments, gameMapGameId, assignmentBooking, filterDate));
    }

    public static ScheduleUpdateModel BuildScheduleUpdateModel(IList<AssignmentModel> assignments,
    IDictionary<int, List<AssignmentModel>> gameMapGameId,
     IList<AssignmentModel> assignmentsBooking,
      DateTime filterDate)
    {
        var textPostfix = ScheduleHelper.BuildTextSingleGamePostfixHdp(gameMapGameId, assignmentsBooking);
        if (string.IsNullOrEmpty(textPostfix))
        {
            textPostfix = ScheduleHelper.BuildTextSingleGamePostfixOu(gameMapGameId, assignmentsBooking);
        }

        var assignmentCell = new AssignmentCell
        {
            Color = ColorCodeConst.BOOKING,
            TextColor = ColorCodeConst.TEXT_BOOKING,
            Text = ScheduleHelper.BuildScheduleTextSingleGame(assignmentsBooking[0], textPostfix)
        };
        var parentGame = assignments.FirstOrDefault(a => a.OWPMatchId == assignmentsBooking[0].ParentMatchId);

        var gameInfo = ScheduleHelper.GetGameInfo(assignmentsBooking);

        var parentGameExtraTime = ETPKHelper.DetectExtraTime(parentGame);
        var etPkValue = (FirstHalfHelper.IsFirstHalf(assignmentsBooking) ||
        assignmentsBooking.All(x => x.BettypeId is Bettype.FifteenMinsBookings)
        || string.Equals(parentGameExtraTime, "PK", StringComparison.OrdinalIgnoreCase))
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