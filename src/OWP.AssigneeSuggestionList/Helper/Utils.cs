using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Helper
{
    public static class Utils
    {
        public static DateTime SelectTime(DateTime src, DateTime dest)
            => src < dest
            ? src
            : dest;

        public static (bool oTPreShift, bool oTPostShift) HasOT(List<AssignmentModel> assignmentModels, WorkingSchedule workingSchedule)
            => (assignmentModels.Exists(x => DateTime.Compare(x.EventDateTime, workingSchedule.UserInfo.ShiftStartTime) < 0)
                , assignmentModels.Exists(x => DateTime.Compare(x.EventDateTime, workingSchedule.UserInfo.ShiftEndTime) > 0));
    }

}