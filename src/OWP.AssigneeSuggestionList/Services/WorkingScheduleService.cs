using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services;

public interface WorkingScheduleService
{
    void UpdateWorkingSchedule(
        ScheduleUpdateModel updateModel,
        WorkingSchedule schedule,
        AssignmentModel assignmentPresent);
}
public class WorkingScheduleServiceImp : WorkingScheduleService
{
    public void UpdateWorkingSchedule(
        ScheduleUpdateModel updateModel,
        WorkingSchedule schedule,
        AssignmentModel assignmentPresent)
    {

        var scheduleProperties = PropertyScheduleHelper.GetValidProperties(
            updateModel.FilterDate,
            updateModel.TimeRange.startDate,
            updateModel.TimeRange.endDate);

        updateModel.PropertiesName = scheduleProperties;
        if (scheduleProperties.Any())
        {
            UpdateWorkingSchedule(schedule, updateModel);
            schedule.CategorizedAssignments.Add(assignmentPresent);
        }

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
}