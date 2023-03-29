using System.Reflection;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services
{
    public interface WorkingScheduleService
    {
        void UpdateWorkingSchedule(
                    DateTime eventDate,
                    IList<int> bettypeIds,
                    AssignmentCell assignmentCell,
                    WorkingSchedule schedule,
                    ETPKUpdateSchedule etPk);
    }
    public class WorkingScheduleServiceImp : WorkingScheduleService
    {
        public void UpdateWorkingSchedule(
            DateTime eventDate,
            IList<int> bettypeIds,
            AssignmentCell assignmentCell,
            WorkingSchedule schedule,
            ETPKUpdateSchedule etPk)
        {
            var (startTime, endTime) = CalculateTimeRangeHelper.CalculateTimeRange(eventDate, bettypeIds, etPk.IsHasEtPk);
            var scheduleProperties = AssigneeSuggestionHelper.GetValidProperties(eventDate, startTime, endTime);

            UpdateWorkingSchedule(schedule,
            new()
            {
                PropertiesName = scheduleProperties,
                AssignmentCell = assignmentCell
            },
            etPk);
        }

        private static void UpdateWorkingSchedule(
            WorkingSchedule schedule,
            WorkingScheduleUpdateModel updateModel,
            ETPKUpdateSchedule etPk)
        {
            var objType = schedule.GetType();
            var assignmentCell = updateModel.AssignmentCell;
            var propertiesName = updateModel.PropertiesName;
            var propertiesLength = propertiesName.Count();
            for (var index = 1; index < propertiesLength; index++)
            {
                var propertyInfo = objType.GetProperty(propertiesName[index], BindingFlags.Public);
                if (propertyInfo is not null)
                {
                    if (index < propertiesLength - 1)
                    {
                        assignmentCell.Text = string.Empty;
                    }
                    if (etPk.IsHasEtPk && index == propertiesLength - 1)
                    {
                        assignmentCell.Text = etPk.EtPkValue;

                    }
                    propertyInfo.SetValue(schedule, assignmentCell);
                }
            }
        }
    }

}