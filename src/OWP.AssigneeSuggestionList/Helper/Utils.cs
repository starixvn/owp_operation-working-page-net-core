using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Helper;

public static class Utils
{
    public static DateTime SelectTime(DateTime src, DateTime dest)
        => src < dest
        ? src
        : dest;

    public static IList<AssignmentModel> GetAssignmentsBelongWs(IEnumerable<AssignmentModel> assignments, WorkingSchedule schedule)
                => assignments.Where(x =>
                    x.UserId == schedule.UserInfo.UserId &&
                    DateTime.Compare(x.EventDateTime, schedule.UserInfo.ShiftStartTime.AddHours(-3)) >= 0 &&
                    DateTime.Compare(x.EventDateTime, schedule.UserInfo.ShiftEndTime.AddHours(3)) <= 0).ToList();

    public static (bool oTPreShift, bool oTPostShift) HasOT(List<AssignmentModel> assignmentModels, WorkingSchedule workingSchedule)
        => (assignmentModels.Exists(x => DateTime.Compare(x.EventDateTime, workingSchedule.UserInfo.ShiftStartTime) < 0)
            , assignmentModels.Exists(x => DateTime.Compare(x.EndTime, workingSchedule.UserInfo.ShiftEndTime) > 0));


    public static bool IsValidSpecialProduct(int? minorBettypeId)
        => minorBettypeId.HasValue && BetTypeSpecial.BetTypes.Contains(minorBettypeId.Value);
}