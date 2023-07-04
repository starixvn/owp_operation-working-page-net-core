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
        List<int> owpIdExclude,
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
        List<int> owpIdExclude,
        IDictionary<int, List<AssignmentModel>> gameMapGameId)
    {
        var gameGroup = assignments
            .Where(a => a.BettypeId is  Bettype.FifteenMinsBookings or Bettype.NextBooking or Bettype.TotalBooking )
            .ToList();
        owpIdExclude.AddRange(gameGroup.Select(x => x.OWPMatchId).Distinct());

        workingSchedule.Group(
           gameGroup,
           schedules,
            assignmentBooking => BuildScheduleUpdateModel(gameMapGameId,assignmentBooking, filterDate));
    }

    public static ScheduleUpdateModel BuildScheduleUpdateModel(IDictionary<int, List<AssignmentModel>> gameMapGameId,IList<AssignmentModel> assignmentsBooking, DateTime filterDate)
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
            Text = ScheduleHelper.BuildScheduleTextSingleGame(assignmentsBooking.First(), textPostfix)
        };

        var gameInfo = ScheduleHelper.GetGameInfo(assignmentsBooking);
        var etPkValue = FirstHalfHelper.IsFirstHalf(assignmentsBooking) ? string.Empty
            : ETPKHelper.DetectExtraTime(gameInfo.remark, gameInfo.minorBettypeId);

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