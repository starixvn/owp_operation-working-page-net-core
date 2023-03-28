using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services
{
    public interface WorkingScheduleCreatorService
    {
        void UpdateWorkingSchedule(
                    DateTime eventDate,
                    IList<int> bettypeIds,
                    AssignmentCell assignmentCell,
                    WorkingSchedule schedule);
    }
    public class WorkingScheduleCreatorServiceImp : WorkingScheduleCreatorService
    {
        public void UpdateWorkingSchedule(
            DateTime eventDate,
            IList<int> bettypeIds,
            AssignmentCell assignmentCell,
            WorkingSchedule schedule)
        {
            var (startTime, endTime) = CalculateTimeRangeHelper.CalculateTimeRange(eventDate, bettypeIds, true);
            var scheduleProperties = AssigneeSuggestionHelper.GetValidProperties(eventDate, startTime, endTime);

            UpdateWorkingScheduleHelper.UpdateWorkingSchedule(schedule,
            new()
            {
                PropertiesName = scheduleProperties,
                AssignmentCell = assignmentCell
            });
        }
    }
}