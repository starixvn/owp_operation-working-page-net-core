using System;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;
using System.Reflection;

namespace OWP.AssigneeSuggestionList.Services;

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

        var scheduleProperties = PropertyScheduleHelper.GetValidProperties(
            updateModel.EventDate,
            updateModel.TimeRange.startDate,
            updateModel.TimeRange.endDate);

        updateModel.PropertiesName = scheduleProperties;
        UpdateWorkingSchedule(schedule, updateModel);
    }

        public static void UpdateWorkingSchedule(
        WorkingSchedule schedule,
        ScheduleUpdateModel updateModel)
    {
        var objType = schedule.GetType();
        var assignmentCell = updateModel.AssignmentCell;
        var propertiesName = updateModel.PropertiesName;
        var propertiesLength = propertiesName.Count;
            objType.GetProperty(propertiesName[0])?.SetValue(schedule, assignmentCell);
        for (var index = 1; index < propertiesLength; index++)
        {
                var propertyInfo = objType.GetProperty(propertiesName[index]);

                if (index <= propertiesLength - 1)
            {
                    propertyInfo?.SetValue(schedule, new AssignmentCell
            }

            {
                        Text = string.Empty,
                        Color = assignmentCell.Color,
                        TextColor = assignmentCell.TextColor
                    });
                }

                if (updateModel.EtPk.IsHasEtPk && index == propertiesLength - 1)
                {
                    propertyInfo?.SetValue(schedule, new AssignmentCell
                    {
                        Text = updateModel.EtPk.EtPkValue,
                        Color = ColorCodeConst.ET_PK,
                        TextColor = ColorCodeConst.TEXT_ET_PK
                    });
        }

    }

}