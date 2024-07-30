using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;
using System.Reflection;

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
        if (!scheduleProperties.Any())
        {
            return;
        }

        UpdateWorkingSchedule(schedule, updateModel);
        schedule.CategorizedAssignments.Add(assignmentPresent);

    }

    public static void UpdateWorkingSchedule(
    WorkingSchedule schedule,
    ScheduleUpdateModel updateModel)
    {
        var objType = schedule.GetType();
        var assignmentCell = updateModel.AssignmentCell;
        var propertiesName = updateModel.PropertiesName;
        var propertiesLength = propertiesName.Count;
        UpdatePropertyValue(objType.GetProperty(propertiesName[0]), schedule, new AssignmentCell
        {
            Text = assignmentCell.Text,
            Color = assignmentCell.Color,
            TextColor = assignmentCell.TextColor,
            KoAssignment = updateModel.TimeRange.startDate
        });

        for (var index = 1; index < propertiesLength; index++)
        {
            var propertyInfo = objType.GetProperty(propertiesName[index]);
            if (index < propertiesLength - 1)//NOSONAR
            {
                UpdatePropertyValue(propertyInfo, schedule, new AssignmentCell
                {
                    Text = string.Empty,
                    Color = assignmentCell.Color,
                    TextColor = assignmentCell.TextColor,
                    KoAssignment = updateModel.TimeRange.startDate
                });
            }

            if (index == propertiesLength - 1)
            {
                UpdatePropertyValue(propertyInfo, schedule, updateModel.EtPk.IsHasEtPk
                    ? new AssignmentCell
                    {
                        Text = updateModel.EtPk.EtPkValue,
                        Color = ColorCodeConst.ET_PK,
                        TextColor = ColorCodeConst.TEXT_ET_PK,
                        KoAssignment = updateModel.TimeRange.startDate
                    }
                    : new AssignmentCell
                    {
                        Text = string.Empty,
                        Color = assignmentCell.Color,
                        TextColor = assignmentCell.TextColor,
                        KoAssignment = updateModel.TimeRange.startDate
                    });
            }
        }
    }
    public static void UpdatePropertyValue(PropertyInfo info, WorkingSchedule schedule, AssignmentCell cellInfo)
    {
        var cells = (List<AssignmentCell>)info?.GetValue(schedule) ?? [];
        cells.Add(cellInfo);
        info?.SetValue(schedule, cells.FilterAndSortValidCell());
    }
}