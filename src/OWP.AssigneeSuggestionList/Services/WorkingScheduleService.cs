using System.Reflection;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services
{
    public interface WorkingScheduleService
    {
        void UpdateWorkingSchedule(
           ScheduleUpdateModel updateModel,
            WorkingSchedule schedule);
    }
    public class WorkingScheduleServiceImp : WorkingScheduleService
    {
        public void UpdateWorkingSchedule(
           ScheduleUpdateModel updateModel,
            WorkingSchedule schedule)
        {

            var scheduleProperties = AssigneeSuggestionHelper.GetValidProperties(
                updateModel.EventDate,
                updateModel.TimeRange.startDate,
                updateModel.TimeRange.endDate);

            updateModel.PropertiesName = scheduleProperties;
            UpdateWorkingSchedule(schedule, updateModel);
        }

        private static void UpdateWorkingSchedule(
            WorkingSchedule schedule,
            ScheduleUpdateModel updateModel)
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
                    if (updateModel.EtPk.IsHasEtPk && index == propertiesLength - 1)
                    {
                        assignmentCell.Text = updateModel.EtPk.EtPkValue;
                        assignmentCell.Color = ColorCodeConst.ET_PK;
                        assignmentCell.TextColor = ColorCodeConst.TEXT_ET_PK;
                    }
                    propertyInfo.SetValue(schedule, assignmentCell);
                }
            }
        }
    }

}